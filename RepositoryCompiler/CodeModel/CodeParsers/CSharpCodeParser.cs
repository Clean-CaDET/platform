using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RepositoryCompiler.CodeModel.CaDETModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.Operations;

namespace RepositoryCompiler.CodeModel.CodeParsers
{
    //TODO: Refactor to extract metric calculation or rename class as it does more than code parsing.
    //TODO: Consider separating class parsing from method parsing.
    public class CSharpCodeParser : ICodeParser
    {
        private CSharpCompilation _compilation;
        private const string _separator = ".";

        public CSharpCodeParser()
        {
            _compilation = CSharpCompilation.Create(new Guid().ToString());
        }

        public List<CaDETClass> GetParsedClasses(IEnumerable<string> sourceCode)
        {
            ParseSyntaxTrees(sourceCode);
            List<CaDETClass> retVal = ParseClasses();
            return LinkClasses(retVal);
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
            parsedClass.Methods = ParseMethods(node.Members, parsedClass, semanticModel);
            parsedClass.Fields = ParseFields(node.Members, parsedClass);
            //TODO: Extract into method CalculateClassMetrics. Consider creating ClassMetrics object.
            parsedClass.MetricLOC = CalculateLinesOfCode(node.ToString());
            parsedClass.MetricLCOM = CalculateLackOfCohesion();

            return parsedClass;
        }

        private double CalculateLackOfCohesion()
        {

            return null;
        }

        private List<CaDETMember> ParseFields(IEnumerable<MemberDeclarationSyntax> nodeMembers, CaDETClass parent)
        {
            List<CaDETMember> fields = new List<CaDETMember>();
            foreach (var node in nodeMembers)
            {
                if (!(node is FieldDeclarationSyntax fieldDeclaration)) continue;
                fields.AddRange(fieldDeclaration.Declaration.Variables.Select(
                    field => new CaDETMember
                    {
                        Name = field.Identifier.Text,
                        Parent = parent,
                        Type = CaDETMemberType.Field
                    }));
            }
            
            return fields;
        }
        private List<CaDETMember> ParseMethods(IEnumerable<MemberDeclarationSyntax> members, CaDETClass parent, SemanticModel semanticModel)
        {
            var methods = new List<CaDETMember>();
            foreach (var member in members)
            {
                CaDETMember method = CreateMethodBasedOnMemberType(member);
                if(method == null) continue;
                method.SourceCode = member.ToString();
                method.Parent = parent;
                method.MetricCYCLO = CalculateCyclomaticComplexity(member);
                method.MetricLOC = CalculateLinesOfCode(member.ToString());
                method.InvokedMethods = CalculateInvokedMethods(member, semanticModel);
                method.AccessedFields = CalculateAccessedFields(member, semanticModel);
                methods.Add(method);
            }
            return methods;
        }

        private int CalculateLinesOfCode(string code)
        {
            return code.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
        }

        private List<CaDETMember> CalculateInvokedMethods(MemberDeclarationSyntax member, SemanticModel semanticModel)
        {
            List<CaDETMember> methods = new List<CaDETMember>();
            var invokedMethods = member.DescendantNodes().OfType<InvocationExpressionSyntax>();
            foreach (var invoked in invokedMethods)
            {
                var symbol = semanticModel.GetSymbolInfo(invoked.Expression).Symbol;
                if(symbol == null) continue; //True when invoked method is a system or library call and not part of our code.
                //Create stub method that will be replaced when all classes are parsed.
                methods.Add(new CaDETMember { Name = symbol.ContainingType + _separator + symbol.Name });
            }

            return methods;
        }
        private List<CaDETMember> CalculateAccessedFields(MemberDeclarationSyntax member, SemanticModel semanticModel)
        {
            List<CaDETMember> fields = new List<CaDETMember>();
            var accessedFields = semanticModel.GetOperation(member).Descendants().OfType<IMemberReferenceOperation>();
            foreach (var field in accessedFields)
            {
                fields.Add(new CaDETMember {Name = field.Member.ToDisplayString()});
            }
            return fields;
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
        private int CalculateCyclomaticComplexity(MemberDeclarationSyntax method)
        {
            //Concretely, in C# the CC of a method is 1 + {the number of following expressions found in the body of the method}:
            //if | while | for | foreach | case | default | continue | goto | && | || | catch | ternary operator ?: | ??
            //https://www.ndepend.com/docs/code-metrics#CC
            int count = method.DescendantNodes().OfType<IfStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<WhileStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<ForStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<ForEachStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<CaseSwitchLabelSyntax>().Count();
            count += method.DescendantNodes().OfType<DefaultSwitchLabelSyntax>().Count();
            count += method.DescendantNodes().OfType<ContinueStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<GotoStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<ConditionalExpressionSyntax>().Count();
            count += method.DescendantNodes().OfType<CatchClauseSyntax>().Count();

            count += CountOccurrences(method.ToString(), "&&");
            count += CountOccurrences(method.ToString(), "||");
            count += CountOccurrences(method.ToString(), "??");
            
            return count + 1;
        }

        private int CountOccurrences(string text, string pattern)
        {
            var count = 0;
            var i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
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

        private List<CaDETClass> LinkClasses(List<CaDETClass> classes)
        {
            foreach (var c in classes)
            {
                foreach (var method in c.Methods)
                {
                    if (method.InvokedMethods == null) continue;
                    method.InvokedMethods = LinkInvokedMembers(classes, method.InvokedMethods);
                    method.AccessedFields = LinkInvokedMembers(classes, method.AccessedFields);
                }
            }
            return classes;
        }

        private List<CaDETMember> LinkInvokedMembers(List<CaDETClass> classes, List<CaDETMember> stubMembers)
        {
            List<CaDETMember> linkedMembers = new List<CaDETMember>();
            foreach (var member in stubMembers)
            {
                string[] nameParts = member.Name.Split(_separator);
                string className = string.Join(_separator, nameParts, 0, nameParts.Length - 1);
                string memberName = nameParts.Last();
                var linkingClass = classes.Find(c => c.FullName.Equals(className));
                linkedMembers.Add(linkingClass.Methods.Find(m => m.Name.Equals(memberName)));
                linkedMembers.Add(linkingClass.Fields.Find(m => m.Name.Equals(memberName)));
            }

            return linkedMembers;
        }
    }
}