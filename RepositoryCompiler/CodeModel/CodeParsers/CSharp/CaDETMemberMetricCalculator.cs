using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
                LOC = CountLinesOfText(member.ToString()),
                ELOC = GetEffectiveLinesOfCode(member),
                NOP = GetNumberOfParameters(method),
                NOLV = GetNumberOfLocalVariables(member),
                NOTC = CountTryCatchBlocks(member),
                NOL = CountLoops(member),
                NOR = CountReturnStatements(member),
                NOC = CountComparisonOperators(member),
                NOMI = CountMethodInvocations(member),
                RFC = CountUniqueMethodInvocations(member),
                NOA = CountNumberOfAssignments(member),
            };
        }

        /// <summary>
        /// DOI: 10.1109/MALTESQUE.2017.7882011
        /// </summary>
        private int GetEffectiveLinesOfCode(MemberDeclarationSyntax member)
        {
            //string[] allLines = member.ToString().Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);
            var allCode = member.ToString();
            
            var allComments = member.DescendantTrivia().Where(t => t.IsKind(SyntaxKind.SingleLineCommentTrivia) || t.IsKind(SyntaxKind.MultiLineCommentTrivia));
            foreach (var comment in allComments)
            {
                allCode = allCode.Replace(comment.ToFullString(), "");
            }
            
            var allLines = allCode.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int blankLines = CountBlankLines(allLines);
            int headerLines = CountHeaderLines(allLines);
            int openAndClosingBracketLines = 2;
            
            return  allLines.Length - (blankLines + headerLines + openAndClosingBracketLines);
        }

        private int CountHeaderLines(string[] allLines)
        {
            int counter = 0;
            foreach (var line in allLines)
            {
                if(line.Contains("{")) break;
                counter++;
            }

            return counter;
        }

        private int CountBlankLines(string[] allLines)
        {
            return allLines.Select(t => t.Trim()).Count(line => line == "");
        }

        /// <summary>
        /// DOI: 10.1002/smr.2255
        /// </summary>
        private int GetNumberOfLocalVariables(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<VariableDeclarationSyntax>().Count();
        }

        /// <summary>
        /// DOI: 10.1002/smr.2255
        /// </summary>
        private int GetNumberOfParameters(CaDETMember method)
        {
            return method.Params.Count;
        }

        public int CountLinesOfText(string code)
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

        private int CountTryCatchBlocks(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<TryStatementSyntax>().Count();
        }

        private int CountLoops(MemberDeclarationSyntax method)
        {
            int count = method.DescendantNodes().OfType<ForEachStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<ForStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<WhileStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<DoStatementSyntax>().Count();
            return count;
        }

        private int CountReturnStatements(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<ReturnStatementSyntax>().Count();
        }

        private int CountComparisonOperators(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<BinaryExpressionSyntax>()
                .Count(n => n.IsKind(SyntaxKind.EqualsExpression) || 
                            n.IsKind(SyntaxKind.NotEqualsExpression) ||
                            n.IsKind(SyntaxKind.LessThanExpression) ||
                            n.IsKind(SyntaxKind.LessThanOrEqualExpression) ||
                            n.IsKind(SyntaxKind.GreaterThanExpression) ||
                            n.IsKind(SyntaxKind.GreaterThanOrEqualExpression));
        }

        private int CountMethodInvocations(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<InvocationExpressionSyntax>().Count();
        }

        private int CountUniqueMethodInvocations(MemberDeclarationSyntax method)
        {
            List<string> uniqueInvokedMethods = new List<string>();
            var allInvokedMethods = method.DescendantNodes().OfType<InvocationExpressionSyntax>();
            foreach (var invokedMethod in allInvokedMethods)
            {
                if (!uniqueInvokedMethods.Contains(invokedMethod.Expression.ToString()))
                {
                    uniqueInvokedMethods.Add(invokedMethod.Expression.ToString());
                }
            }
            return uniqueInvokedMethods.Count();
        }

        private int CountNumberOfAssignments(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<AssignmentExpressionSyntax>().Count();
        }
    }
}