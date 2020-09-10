using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RepositoryCompiler.CodeModel.CaDETModel;

namespace RepositoryCompiler.CodeModel.SyntaxParsers
{
    public class CSharpSyntaxParser : ISyntaxParser
    {
        public IEnumerable<CaDETClass> ParseClasses(string sourceCode)
        {
            SyntaxTree ast = CSharpSyntaxTree.ParseText(sourceCode);
            SyntaxNode root = ast.GetRoot();

            var classNodes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            foreach (var node in classNodes)
            {
                CaDETClass parsedClass = new CaDETClass();
                parsedClass.Name = node.Identifier.Text;
            }
            return null;
        }
    }
}