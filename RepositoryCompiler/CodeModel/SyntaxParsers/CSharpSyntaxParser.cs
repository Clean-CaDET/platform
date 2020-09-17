using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RepositoryCompiler.CodeModel.CaDETModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryCompiler.CodeModel.SyntaxParsers
{
    public class CSharpSyntaxParser : ISyntaxParser
    {
        public IEnumerable<CaDETClass> ParseClasses(string sourceCode)
        {
            SyntaxTree ast = CSharpSyntaxTree.ParseText(sourceCode);
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

        private CaDETMethod ParseMethod(MethodDeclarationSyntax method, CaDETClass parent)
        {
            return new CaDETMethod
            {
                IsAccessor = false,
                IsConstructor = false,
                Name = method.Identifier.Text,
                SourceCode = method.ToString(),
                Parent = parent
            };
        }

        private CaDETMethod ParseConstructor(ConstructorDeclarationSyntax constructor, CaDETClass parent)
        {
            return new CaDETMethod
            {
                IsAccessor = false,
                IsConstructor = true,
                Name = constructor.Identifier.Text,
                SourceCode = constructor.ToString(),
                Parent = parent
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
    }
}