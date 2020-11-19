using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompiler.CodeModel.CaDETModel.Metrics;

namespace RepositoryCompiler.CodeModel.CodeParsers.CSharp
{
    internal class CaDETMemberMetricCalculator
    {
        internal CaDETMemberMetrics CalculateMemberMetrics(MemberDeclarationSyntax member, CaDETMember method)
        {
            return new CaDETMemberMetrics
            {
                CYCLO = CalculateCyclomaticComplexity(member),
                LOC = GetLinesOfCode(member.ToString()),
                NOP = GetNumberOfParameters(method),
                NOLV = GetNumberOfLocalVariables(member)
            };
        }

        /// <summary>
        /// DOI: 10.1002/smr.2255
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private int GetNumberOfLocalVariables(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<VariableDeclarationSyntax>().Count();
        }

        /// <summary>
        /// DOI: 10.1002/smr.2255
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private int GetNumberOfParameters(CaDETMember method)
        {
            return method.Params.Count;
        }

        public int GetLinesOfCode(string code)
        {
            return code.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
        }

        private int CalculateCyclomaticComplexity(MemberDeclarationSyntax method)
        {
            //Defined based on https://www.ndepend.com/docs/code-metrics#CC
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

            count += CountLogicalOperators(method, "&&");
            count += CountLogicalOperators(method, "||");
            count += CountLogicalOperators(method, "??");
            
            return count + 1;
        }

        private int CountLogicalOperators(MemberDeclarationSyntax method, string pattern)
        {
            var comments = method.DescendantTrivia();
            int commentOperatorCount = comments.Sum(comment => CountOccurrences(comment.ToString(), pattern));
            return CountOccurrences(method.ToString(), pattern) - commentOperatorCount;
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