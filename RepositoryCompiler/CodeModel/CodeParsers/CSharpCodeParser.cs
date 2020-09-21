using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RepositoryCompiler.CodeModel.CaDETModel;

namespace RepositoryCompiler.CodeModel.CodeParsers
{
    public class CSharpCodeParser : ICodeParser
    {
        private readonly CSharpCompilation _compilation; 

        public CSharpCodeParser()
        {
            _compilation = CSharpCompilation.Create(new Guid().ToString());
        }
        private void CreateSemanticModel(SyntaxTree ast)
        {
            var semanticModel = _compilation.GetSemanticModel(ast);


            /*var invokedMethods = method.DescendantNodes().OfType<InvocationExpressionSyntax>();
            foreach (var invoked in invokedMethods)
            {
                var a = invoked;
                var symbol = _semanticModel.GetSymbolInfo(a.Expression).Symbol as IMethodSymbol;
                var test = symbol;
            }*/
        }

        public List<CaDETClass> ParseClasses(string sourceCode)
        {
            SyntaxTree ast = CSharpSyntaxTree.ParseText(sourceCode);
            _compilation.AddSyntaxTrees(ast);

            SyntaxNode root = ast.GetRoot();

            var classNodes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            return classNodes.Select(ParseClass).ToList();
        }

        private CaDETClass ParseClass(ClassDeclarationSyntax node)
        {
            var parsedClass = new CaDETClass
            {
                Name = node.Identifier.Text,
                FullName = GetFullName(node),
                SourceCode = node.ToString()
            };
            parsedClass.Methods = ParseMethods(node.Members, parsedClass);
            parsedClass.Fields = ParseFields(node.Members, parsedClass);
            return parsedClass;
        }

        private string GetFullName(ClassDeclarationSyntax node)
        {
            var classPart = GetClassAndInnerClassPart(node);
            var namespacePart = GetNamespacePart(node);
            
            return namespacePart + classPart;
        }

        private string GetClassAndInnerClassPart(ClassDeclarationSyntax node)
        {
            var sb = new StringBuilder();
            const string delimiter = "+";

            if (node.Parent is ClassDeclarationSyntax parentClass)
                sb.Append(GetClassAndInnerClassPart(parentClass)).Append(delimiter);
            sb.Append(node.Identifier.Text);
            return sb.ToString();
        }
        private string GetNamespacePart(ClassDeclarationSyntax node)
        {
            var parent = node.Parent;
            while (parent is ClassDeclarationSyntax) parent = parent.Parent;

            var retVal = (NamespaceDeclarationSyntax) parent;
            return retVal.Name + ".";
        }

        private List<CaDETField> ParseFields(IEnumerable<MemberDeclarationSyntax> nodeMembers, CaDETClass parent)
        {
            List<CaDETField> fields = new List<CaDETField>();
            foreach (var node in nodeMembers)
            {
                if (!(node is FieldDeclarationSyntax fieldDeclaration)) continue;
                fields.AddRange(fieldDeclaration.Declaration.Variables.Select(
                    field => new CaDETField {Name = field.Identifier.Text, Parent = parent}));
            }
            
            return fields;
        }

        private List<CaDETMethod> ParseMethods(IEnumerable<MemberDeclarationSyntax> nodeMembers, CaDETClass parent)
        {
            var methods = new List<CaDETMethod>();
            foreach (var node in nodeMembers)
            {
                switch (node)
                {
                    case PropertyDeclarationSyntax property:
                        methods.Add(ParseProperty(property, parent));
                        break;
                    case ConstructorDeclarationSyntax constructor:
                        methods.Add(ParseConstructor(constructor, parent));
                        break;
                    case MethodDeclarationSyntax method:
                        methods.Add(ParseMethod(method, parent));
                        break;
                }
            }
            return methods;
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

        private CaDETMethod ParseMethod(MethodDeclarationSyntax method, CaDETClass parent)
        {
            return new CaDETMethod
            {
                IsAccessor = false,
                IsConstructor = false,
                Name = method.Identifier.Text,
                SourceCode = method.ToString(),
                Parent = parent,
                MetricCYCLO = CalculateCyclomaticComplexity(method)
            };
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

        private CaDETMethod ParseConstructor(ConstructorDeclarationSyntax constructor, CaDETClass parent)
        {
            return new CaDETMethod
            {
                IsAccessor = false,
                IsConstructor = true,
                Name = constructor.Identifier.Text,
                SourceCode = constructor.ToString(),
                Parent = parent,
                MetricCYCLO = CalculateCyclomaticComplexity(constructor)
            };
        }

        private CaDETMethod ParseProperty(PropertyDeclarationSyntax property, CaDETClass parent)
        {
            return new CaDETMethod
            {
                IsAccessor = true,
                IsConstructor = false,
                Name = property.Identifier.Text,
                SourceCode = property.ToString(),
                Parent = parent
            };
        }

        public List<CaDETClass> CalculateSemanticMetrics(List<CaDETClass> classes)
        {
            throw new NotImplementedException();
        }
    }
}