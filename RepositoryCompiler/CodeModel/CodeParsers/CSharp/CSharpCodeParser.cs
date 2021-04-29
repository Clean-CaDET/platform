using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RepositoryCompiler.CodeModel.CodeParsers.CSharp.Exceptions;


namespace RepositoryCompiler.CodeModel.CodeParsers.CSharp
{
    public class CSharpCodeParser : ICodeParser
    {
        private CSharpCompilation _compilation;
        private readonly CaDETClassMetricCalculator _metricCalculator;
        private readonly Dictionary<CaDETClass, List<CSharpCaDETMemberBuilder>> _memberBuilders;

        public CSharpCodeParser()
        {
            _compilation = CSharpCompilation.Create(new Guid().ToString());
            _metricCalculator = new CaDETClassMetricCalculator();
            _memberBuilders = new Dictionary<CaDETClass, List<CSharpCaDETMemberBuilder>>();
        }

        public List<CaDETClass> GetParsedClasses(IEnumerable<string> sourceCode)
        {
            LoadSyntaxTrees(sourceCode);
            var parsedClasses = ParseClasses();
            ValidateUniqueFullNameForNonPartial(parsedClasses);
            parsedClasses = ConnectCaDETGraph(parsedClasses);
            return CalculateMetrics(parsedClasses);
        }

        private void LoadSyntaxTrees(IEnumerable<string> sourceCode)
        {
            foreach (var code in sourceCode)
            {
                _compilation = _compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(code));
            }
        }
        private List<CaDETClass> ParseClasses()
        {
            List<CaDETClass> builtClasses = new List<CaDETClass>();
            foreach (var ast in _compilation.SyntaxTrees)
            {
                var semanticModel = _compilation.GetSemanticModel(ast);
                var classNodes = ast.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();

                foreach (var node in classNodes)
                {
                    try
                    {
                        ValidateNoPartialModifier(node);
                        ValidateNoStructParent(node);
                        builtClasses.Add(ParseClass(semanticModel, node));
                    }
                    catch (PartialIsNotSupportedException)
                    {
                        // Skips classes with partial keyword.
                    }
                    catch (StructIsNotSupportedException)
                    {
                        //Skips members belonging to structs.
                    }
                }
            }
            return builtClasses;
        }
        private static void ValidateNoStructParent(ClassDeclarationSyntax node)
        {
            var parentNode = node.Parent;
            while (parentNode != null)
            {
                if (parentNode is StructDeclarationSyntax) throw new StructIsNotSupportedException();
                parentNode = parentNode.Parent;
            }
        }

        private CaDETClass ParseClass(SemanticModel semanticModel, ClassDeclarationSyntax node)
        {
            var symbol = semanticModel.GetDeclaredSymbol(node);
            var parsedClass = new CaDETClass
            {
                Name = symbol.Name,
                FullName = symbol.ToDisplayString(),
                SourceCode = node.ToString()
            };
            parsedClass.Modifiers = GetModifiers(node);
            parsedClass.Parent = new CaDETClass { Name = symbol.BaseType.ToString() };
            parsedClass.Fields = ParseFields(node.Members, parsedClass, semanticModel);
            parsedClass.Members = ParseMethods(node.Members, parsedClass, semanticModel);
            return parsedClass;
        }

        private List<CaDETField> ParseFields(IEnumerable<MemberDeclarationSyntax> nodeMembers, CaDETClass parent, SemanticModel model)
        {
            List<CaDETField> fields = new List<CaDETField>();

            foreach (var node in nodeMembers)
            {
                if (!(node is FieldDeclarationSyntax fieldDeclaration)) continue;
                
                fields.AddRange(fieldDeclaration.Declaration.Variables.Select(
                    field => new CaDETField
                    {
                        Name = field.Identifier.Text,
                        Parent = parent,
                        Modifiers = GetModifiers(node),
                        Type = new CaDETLinkedType() { FullType = ((IFieldSymbol)model.GetDeclaredSymbol(field)).Type.ToString() }
                    }));
            }
            
            return fields;
        }
        private List<CaDETMember> ParseMethods(IEnumerable<MemberDeclarationSyntax> members, CaDETClass parent, SemanticModel semanticModel)
        {
            CreateClassMemberBuilders(parent, members, semanticModel);
            return _memberBuilders[parent].Select(builder => builder.CreateBasicMember(parent)).Where(method => method != null).ToList();
        }

        private void CreateClassMemberBuilders(CaDETClass parent, IEnumerable<MemberDeclarationSyntax> members, SemanticModel semanticModel)
        {
            var classMemberBuilders = new List<CSharpCaDETMemberBuilder>();
            foreach (var member in members)
            {
                try
                {
                    ValidateNoPartialModifier(member);
                    classMemberBuilders.Add(new CSharpCaDETMemberBuilder(member, semanticModel));
                }
                catch (InappropriateMemberTypeException)
                {
                    //MemberDeclarationSyntax is not property, constructor, or method.
                }
                catch (PartialIsNotSupportedException)
                {
                    //Skips members with partial keyword.
                }
            }
            _memberBuilders.Add(parent, classMemberBuilders);
        }

        private static void ValidateNoPartialModifier(MemberDeclarationSyntax member)
        {
            if (member.Modifiers.Any(m => m.ValueText.Equals("partial"))) throw new PartialIsNotSupportedException();
            var memberParent = member.Parent;
            while (memberParent is MemberDeclarationSyntax parent)
            {
                if (parent.Modifiers.Any(m => m.ValueText.Equals("partial"))) throw new PartialIsNotSupportedException();
                memberParent = parent.Parent;
            }
        }

        private List<CaDETModifier> GetModifiers(MemberDeclarationSyntax member)
        {
            return member.Modifiers.Select(modifier => new CaDETModifier(modifier.ValueText)).ToList();
        }
        private void ValidateUniqueFullNameForNonPartial(List<CaDETClass> parsedClasses)
        {
            var nonUniqueFullNameClasses = new List<CaDETClass>();
            for (int i = 0; i < parsedClasses.Count - 1; i++)
            {
                if(parsedClasses[i].IsPartialClass()) continue;
                for (int j = i+1; j < parsedClasses.Count; j++)
                {
                    if (parsedClasses[i].FullName.Equals(parsedClasses[j].FullName))
                    {
                        nonUniqueFullNameClasses.Add(parsedClasses[i]);
                    }
                }
            }

            if (nonUniqueFullNameClasses.Count > 0)
            {
                throw new NonUniqueFullNameException(nonUniqueFullNameClasses.ToArray().ToString());
            }

        }

        private List<CaDETClass> ConnectCaDETGraph(List<CaDETClass> classes)
        {
            foreach (var c in classes)
            {
                c.Parent = LinkParent(classes, c.Parent);
                c.OuterClass = LinkOuterClass(classes, c.ContainerName);
                c.Fields = LinkFields(classes, c.Fields);
                c.Members = LinkMembers(classes, c.Members);
                foreach (var memberBuilder in _memberBuilders[c])
                {
                    memberBuilder.DetermineAccessedCodeItems(classes);
                }
            }
            return classes;
        }

        private CaDETClass LinkParent(List<CaDETClass> classes, CaDETClass parent)
        {
            if (parent.Name.Equals("object")) return null;
            return classes.FirstOrDefault(c => c.FullName.Equals(parent.Name));
        }
        private CaDETClass LinkOuterClass(List<CaDETClass> classes, string containerName)
        {
            return classes.FirstOrDefault(c => c.FullName.Equals(containerName));
        }

        private List<CaDETField> LinkFields(List<CaDETClass> classes, List<CaDETField> fields)
        {
            List<CaDETField> linkedFields = new List<CaDETField>();
            foreach (var f in fields)
            {
                f.Type.LinkedTypes = GetTypes(classes, f.Type);
                linkedFields.Add(f);
            }
            
            return linkedFields;
        }

        private List<CaDETMember> LinkMembers(List<CaDETClass> classes, List<CaDETMember> members)
        {
            List<CaDETMember> linkedMembers = new List<CaDETMember>();
            foreach (var m in members)
            {
                if (!(m.Type is CaDETMemberType.Property))
                {
                    m.Variables = LinkMethodVariables(classes, m);
                }

                if (!(m.Type is CaDETMemberType.Constructor))
                {
                    m.ReturnType ??= new CaDETLinkedType();
                    m.ReturnType.LinkedTypes = GetTypes(classes, m.ReturnType);
                }

                linkedMembers.Add(m);
            }
            return linkedMembers;
        }

        private List<CaDETVariable> LinkMethodVariables(List<CaDETClass> classes, CaDETMember member)
        {
            List<CaDETVariable> linkedVariables = new List<CaDETVariable>();
            foreach (var variable in member.Variables)
            {
                var variableTypes = GetTypes(classes, variable.Type);
                if (variableTypes != null) variable.Type.LinkedTypes = variableTypes;
                linkedVariables.Add(variable);
            }
            return linkedVariables;
        }

        private List<CaDETClass> GetTypes(List<CaDETClass> classes, CaDETLinkedType type)
        {
            List<CaDETClass> types = new List<CaDETClass>();
            if (type != null)
            {
                types.Add(CheckForSingleOrArrayType(classes, type.FullType));
                types.AddRange(GetCollectionTypes(classes, type.FullType));
            }
            return types.Where(t => t != null).ToList();
        }

        private CaDETClass CheckForSingleOrArrayType(List<CaDETClass> classes, string fullType)
        {
            if (fullType == null) return null;
            var foundType = classes.FirstOrDefault(c => c.FullName.Equals(fullType));
            if (foundType != null) return foundType;
            foundType = GetArrayType(classes, fullType);
            return foundType;
        }
        
        private List<CaDETClass> GetCollectionTypes(List<CaDETClass> classes, string type)
        {
            List<CaDETClass> collectionTypes = new List<CaDETClass>();
            if (type == null) return collectionTypes;
            var match = Regex.Match(type, "<.+>");
            if (!match.Success) return collectionTypes;
            
            var typeParameterSection = match.Value.Substring(1, match.Value.Length - 2);
            collectionTypes.AddRange(GetCollectionTypes(classes, typeParameterSection));
            var typeParameters = typeParameterSection.Split(",").Select(t => t.Trim()).ToList();
            foreach (var t in typeParameters)
            {
                collectionTypes.Add(CheckForSingleOrArrayType(classes, t));
            }

            return collectionTypes.Where(ct => ct != null).Distinct().ToList();
        }

        private CaDETClass GetArrayType(List<CaDETClass> classes, string type)
        {
            if (type == null) return null;
            if (!Regex.IsMatch(type, "\\[.*\\]")) return null;
            
            var typeName = type.Split("[")[0];
            return classes.FirstOrDefault(c => c.FullName.Equals(typeName));
        }

        private List<CaDETClass> CalculateMetrics(List<CaDETClass> linkedClasses)
        {
            foreach (var c in linkedClasses)
            {
                foreach (var memberBuilder in _memberBuilders[c])
                {
                    memberBuilder.CalculateMetrics();
                }
                c.Metrics = _metricCalculator.CalculateClassMetrics(c);
            }

            return linkedClasses;
        }
    }
}