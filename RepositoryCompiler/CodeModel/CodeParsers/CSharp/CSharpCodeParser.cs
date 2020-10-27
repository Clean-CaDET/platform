using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System;
using System.Collections.Generic;
using System.Linq;


namespace RepositoryCompiler.CodeModel.CodeParsers.CSharp
{
    public class CSharpCodeParser : ICodeParser
    {
        private CSharpCompilation _compilation;
        private readonly CSharpMetricCalculator _metricCalculator;
        private const string _separator = ".";

        public CSharpCodeParser()
        {
            _compilation = CSharpCompilation.Create(new Guid().ToString());
            _metricCalculator = new CSharpMetricCalculator();
        }

        public List<CaDETClass> GetParsedClasses(IEnumerable<string> sourceCode)
        {
            ParseSyntaxTrees(sourceCode);
            var parsedClasses = ParseClasses();
            var linkedClasses = LinkClasses(parsedClasses);
            return AddClassMetrics(linkedClasses);
        }

        private void ParseSyntaxTrees(IEnumerable<string> sourceCode)
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
                    builtClasses.Add(ParseClass(semanticModel, node));
                }
            }
            return builtClasses;
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
            parsedClass.Fields = ParseFields(node.Members, parsedClass);
            parsedClass.Members = ParseMethodsAndCalculateMetrics(node.Members, parsedClass, semanticModel);

            return parsedClass;
        }

        private List<CaDETField> ParseFields(IEnumerable<MemberDeclarationSyntax> nodeMembers, CaDETClass parent)
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
                        Modifiers = GetModifiers(node)
                    }));
            }
            
            return fields;
        }
        private List<CaDETMember> ParseMethodsAndCalculateMetrics(IEnumerable<MemberDeclarationSyntax> members, CaDETClass parent, SemanticModel semanticModel)
        {
            var methods = new List<CaDETMember>();
            foreach (var member in members)
            {
                CaDETMember method = CreateMethodBasedOnMemberType(member);
                if(method == null) continue;
                method.Modifiers = GetModifiers(member);
                method.SourceCode = member.ToString();
                method.Parent = parent;
                method.InvokedMethods = CalculateInvokedMethods(member, semanticModel);
                method.AccessedFields = CalculateAccessedFields(member, semanticModel);
                method.AccessedAccessors = CalculateAccessedAccessors(member, semanticModel);
                method.Params = GetMethodParams(member);
                // It's important to calculate metrics at the end, because they require the information above.
                method.Metrics = _metricCalculator.CalculateMemberMetrics(member, method);
                methods.Add(method);
            }
            return methods;
        }

        private List<CaDETModifier> GetModifiers(MemberDeclarationSyntax member)
        {
            return member.Modifiers.Select(modifier => new CaDETModifier(modifier.ValueText)).ToList();
        }

        private List<string> GetMethodParams(MemberDeclarationSyntax member)
        {
            List<string> memberParams = new List<string>();
            // We use First because we have others lambda expressions in this parsed paramLists
            // lambda expressions are second, third... but we only need function params and we take it from FIRST
            var paramLists = member.DescendantNodes().OfType<ParameterListSyntax>().ToList();
            if (!paramLists.Any()) return memberParams;

            var parameters = paramLists.First().Parameters;
            memberParams.AddRange(parameters.Select(parameter => parameter.Identifier.ValueText));
            return memberParams;
        }

        private CaDETMember CreateMethodBasedOnMemberType(MemberDeclarationSyntax member)
        {
            return member switch
            {
                PropertyDeclarationSyntax property => ParseProperty(property),
                ConstructorDeclarationSyntax constructor => ParseConstructor(constructor),
                MethodDeclarationSyntax method => ParseMethod(method),
                _ => null
            };
        }

        private CaDETMember ParseMethod(MethodDeclarationSyntax method)
        {
            return new CaDETMember
            {
                Type = CaDETMemberType.Method,
                Name = method.Identifier.Text
            };
        }
        private CaDETMember ParseConstructor(ConstructorDeclarationSyntax constructor)
        {
            return new CaDETMember
            {
                Type = CaDETMemberType.Constructor,
                Name = constructor.Identifier.Text
            };
        }
        private CaDETMember ParseProperty(PropertyDeclarationSyntax property)
        {
            return new CaDETMember
            {
                Type = CaDETMemberType.Property,
                Name = property.Identifier.Text
            };
        }

        private ISet<CaDETMember> CalculateInvokedMethods(MemberDeclarationSyntax member, SemanticModel semanticModel)
        {
            ISet<CaDETMember> methods = new HashSet<CaDETMember>();
            var invokedMethods = member.DescendantNodes().OfType<InvocationExpressionSyntax>();
            foreach (var invoked in invokedMethods)
            {
                var symbol = semanticModel.GetSymbolInfo(invoked.Expression).Symbol;
                if (symbol == null) continue; //True when invoked method is a system or library call and not part of our code.
                //The code below creates a stub method that will be replaced when all classes are parsed and linking is performed.
                methods.Add(new CaDETMember { Name = symbol.ContainingType + _separator + symbol.Name });
            }

            return methods;
        }

        private ISet<CaDETField> CalculateAccessedFields(MemberDeclarationSyntax member, SemanticModel semanticModel)
        {
            ISet<CaDETField> fields = new HashSet<CaDETField>();
            var accessedFields = semanticModel.GetOperation(member).Descendants().OfType<IFieldReferenceOperation>();
            foreach (var field in accessedFields)
            {
                fields.Add(new CaDETField { Name = field.Member.ToDisplayString() });
            }
            return fields;
        }

        private ISet<CaDETMember> CalculateAccessedAccessors(MemberDeclarationSyntax member, SemanticModel semanticModel)
        {
            ISet<CaDETMember> accessors = new HashSet<CaDETMember>();
            var accessedAccessors = semanticModel.GetOperation(member).Descendants().OfType<IPropertyReferenceOperation>();
            foreach (var accessor in accessedAccessors)
            {
                accessors.Add(new CaDETMember { Name = accessor.Member.ToDisplayString() });
            }
            return accessors;
        }

        private List<CaDETClass> LinkClasses(List<CaDETClass> classes)
        {
            foreach (var c in classes)
            {
                c.Parent = LinkParent(classes, c.Parent);
                foreach (var method in c.Members)
                {
                    if (method.InvokedMethods == null) continue;
                    method.InvokedMethods = LinkInvokedMembers(classes, method.InvokedMethods);
                    method.AccessedAccessors = LinkInvokedMembers(classes, method.AccessedAccessors);
                    method.AccessedFields = LinkAccessedFields(classes, method.AccessedFields);
                }
            }
            return classes;
        }

        private CaDETClass LinkParent(List<CaDETClass> classes, CaDETClass parent)
        {
            if (parent.Name.Equals("object")) return null;
            return classes.FirstOrDefault(c => c.FullName.Equals(parent.Name));
        }

        private ISet<CaDETMember> LinkInvokedMembers(List<CaDETClass> classes, ISet<CaDETMember> stubMembers)
        {
            ISet<CaDETMember> linkedMembers = new HashSet<CaDETMember>();
            foreach (var member in stubMembers)
            {
                var containingClass = FindContainingClass(classes, member.Name);
                if(IsEnumeration(containingClass)) continue;
                string memberName = member.Name.Split(_separator).Last();
                var linkedMember = containingClass.FindMember(memberName);
                if (linkedMember != null) linkedMembers.Add(linkedMember);
            }

            return linkedMembers;
        }

        private ISet<CaDETField> LinkAccessedFields(List<CaDETClass> classes, ISet<CaDETField> stubFields)
        {
            ISet<CaDETField> linkedFields = new HashSet<CaDETField>();
            foreach (var field in stubFields)
            {
                var containingClass = FindContainingClass(classes, field.Name);
                if (IsEnumeration(containingClass)) continue;
                string fieldName = field.Name.Split(_separator).Last();
                var linkedField = containingClass.FindField(fieldName);
                if (linkedField != null) linkedFields.Add(linkedField);
            }

            return linkedFields;
        }

        private CaDETClass FindContainingClass(List<CaDETClass> classes, string stubElementName)
        {
            string[] nameParts = stubElementName.Split(_separator);
            string className = string.Join(_separator, nameParts, 0, nameParts.Length - 1);
            return classes.Find(c => c.FullName.Equals(className));
        }

        private bool IsEnumeration(CaDETClass linkingClass)
        {
            return linkingClass == null;
        }

        private List<CaDETClass> AddClassMetrics(List<CaDETClass> linkedClasses)
        {
            foreach (var c in linkedClasses)
            {
                c.Metrics = _metricCalculator.CalculateClassMetrics(c);
            }

            return linkedClasses;
        }
    }
}