using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using RepositoryCompiler.CodeModel.CaDETModel;

namespace RepositoryCompiler.CodeModel.CodeParsers.CSharp
{
    public class CSharpMetricCalculator
    {
        private readonly string _separator;

        public CSharpMetricCalculator(string separator)
        {
            _separator = separator;
        }

        public CaDETClassMetrics CalculateClassMetrics(ClassDeclarationSyntax node, CaDETClass parsedClass)
        {
            return new CaDETClassMetrics
            {
                LOC = CalculateLinesOfCode(node.ToString()),
                NMD = parsedClass.Methods.Count(method => method.Type.Equals(CaDETMemberType.Method)),
                NAD = parsedClass.Fields.Count,
                WMC = parsedClass.Methods.Sum(method => method.Metrics.CYCLO)
            };
        }

        public CaDETMemberMetrics CalculateMemberMetrics(MemberDeclarationSyntax member, SemanticModel semanticModel)
        {
            return new CaDETMemberMetrics
            {
                CYCLO = CalculateCyclomaticComplexity(member),
                LOC = CalculateLinesOfCode(member.ToString()),
                InvokedMethods = CalculateInvokedMethods(member, semanticModel),
                AccessedFieldsAndAccessors = CalculateAccessedFieldsAndAccessors(member, semanticModel)
            };
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

        private List<CaDETMember> CalculateAccessedFieldsAndAccessors(MemberDeclarationSyntax member, SemanticModel semanticModel)
        {
            List<CaDETMember> fields = new List<CaDETMember>();
            var accessedFields = semanticModel.GetOperation(member).Descendants().OfType<IMemberReferenceOperation>();
            foreach (var field in accessedFields)
            {
                fields.Add(new CaDETMember {Name = field.Member.ToDisplayString()});
            }
            return fields;
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
    }
}