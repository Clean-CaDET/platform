using Microsoft.CodeAnalysis.CSharp.Syntax;
using RepositoryCompiler.CodeModel.CaDETModel;
using System;
using System.Linq;

namespace RepositoryCompiler.CodeModel.CodeParsers.CSharp
{
    public class CSharpMetricCalculator
    {
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

        public CaDETMemberMetrics CalculateMemberMetrics(MemberDeclarationSyntax member)
        {
            return new CaDETMemberMetrics
            {
                CYCLO = CalculateCyclomaticComplexity(member),
                LOC = CalculateLinesOfCode(member.ToString())
            };
        }

        private int CalculateLinesOfCode(string code)
        {
            return code.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
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