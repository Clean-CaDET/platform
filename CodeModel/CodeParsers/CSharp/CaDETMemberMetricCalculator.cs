using CodeModel.CaDETModel.CodeItems;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeModel.CodeParsers.CSharp
{
    internal class CaDETMemberMetricCalculator
    {
        internal Dictionary<CaDETMetric, double> CalculateMemberMetrics(MemberDeclarationSyntax member, CaDETMember method)
        {
            return new()
            {
                [CaDETMetric.CYCLO] = CalculateCyclomaticComplexity(member),
                [CaDETMetric.CYCLO_SWITCH] = CalculateCyclomaticComplexityWithoutCases(member),
                [CaDETMetric.MLOC] = CountLinesOfText(member.ToString()),
                [CaDETMetric.MELOC] = GetEffectiveLinesOfCode(member),
                [CaDETMetric.NOP] = GetNumberOfParameters(method),
                [CaDETMetric.NOLV] = GetNumberOfLocalVariables(member),
                [CaDETMetric.NOTC] = CountTryCatchBlocks(member),
                [CaDETMetric.MNOL] = CountLoops(member),
                [CaDETMetric.MNOR] = CountReturnStatements(member),
                [CaDETMetric.MNOC] = CountComparisonOperators(member),
                [CaDETMetric.MNOA] = CountNumberOfAssignments(member),
                [CaDETMetric.NONL] = CountNumberOfNumericLiterals(member),
                [CaDETMetric.NOSL] = CountNumberOfStringLiterals(member),
                [CaDETMetric.NOMO] = CountNumberOfMathOperations(member),
                [CaDETMetric.NOPE] = CountNumberOfParenthesizedExpressions(member),
                [CaDETMetric.NOLE] = CountNumberOfLambdaExpressions(member),
                [CaDETMetric.MMNB] = CountMaxNestedBlocks(member),
                [CaDETMetric.NOUW] = CountNumberOfUniqueWords(member),
                [CaDETMetric.AID] = CountAccessOfImportData(method)
            };
        }

        private static int CountAccessOfImportData(CaDETMember member)
        {
            var accessedExternalFields = member.AccessedFields.Where(f => !f.Parent.Equals(member.Parent));
            var accessedExternalAccessors = member.AccessedAccessors.Where(a => !a.Parent.Equals(member.Parent));
            var usedExternalMethods = member.InvokedMethods.Where(m => !m.Parent.Equals(member.Parent));

            return accessedExternalAccessors.Count() + accessedExternalFields.Count() + usedExternalMethods.Count();
        }

        /// <summary>
        /// DOI: 10.1109/MALTESQUE.2017.7882011
        /// </summary>
        private static int GetEffectiveLinesOfCode(CSharpSyntaxNode node)
        {
            //string[] allLines = member.ToString().Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);
            string allCode = RemoveCommentsFromCode(node);
            
            var allLines = allCode.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int blankLines = CountBlankLines(allLines);
            int headerLines = CountHeaderLines(allLines);
            int openAndClosingBracketLines = 2;
            
            return Math.Max(allLines.Length - (blankLines + headerLines + openAndClosingBracketLines), 1);
        }

        private static string RemoveCommentsFromCode(CSharpSyntaxNode node)
        {
            if (node == null) return ""; // when removing comments from the method body, the body can be empty
            var allCode = node.ToString();
            var allComments = node.DescendantTrivia().Where(t => t.IsKind(SyntaxKind.SingleLineCommentTrivia) || t.IsKind(SyntaxKind.MultiLineCommentTrivia));
            foreach (var comment in allComments)
            {
                allCode = allCode.Replace(comment.ToFullString(), "");
            }
            return allCode;
        }
        

        private static int CountHeaderLines(string[] allLines)
        {
            int counter = 0;
            foreach (var line in allLines)
            {
                if(line.Contains("{")) break;
                counter++;
            }

            return counter;
        }

        private static int CountBlankLines(string[] allLines)
        {
            return allLines.Select(t => t.Trim()).Count(line => line == "");
        }

        /// <summary>
        /// DOI: 10.1002/smr.2255
        /// </summary>
        private static int GetNumberOfLocalVariables(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<VariableDeclarationSyntax>().Count();
        }

        /// <summary>
        /// DOI: 10.1002/smr.2255
        /// </summary>
        private static int GetNumberOfParameters(CaDETMember method)
        {
            return method.Params.Count;
        }

        private static int CountLinesOfText(string code)
        {
            return code.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
        }

        private static int CalculateCyclomaticComplexity(MemberDeclarationSyntax method)
        {
            //Defined based on https://www.ndepend.com/docs/code-metrics#CC
            var count = method.DescendantNodes().OfType<IfStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<WhileStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<ForStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<ForEachStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<CaseSwitchLabelSyntax>().Count();
            count += method.DescendantNodes().OfType<CasePatternSwitchLabelSyntax>().Count();
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
      
        private int CalculateCyclomaticComplexityWithoutCases(MemberDeclarationSyntax method)
        {
             int count = method.DescendantNodes().OfType<IfStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<WhileStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<ForStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<ForEachStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<SwitchStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<ContinueStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<GotoStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<ConditionalExpressionSyntax>().Count();
            count += method.DescendantNodes().OfType<CatchClauseSyntax>().Count();

            count += CountLogicalOperators(method, "&&");
            count += CountLogicalOperators(method, "||");
            count += CountLogicalOperators(method, "??");

            return count + 1;
        }

        private static int CountLogicalOperators(MemberDeclarationSyntax method, string pattern)
        {
            var comments = method.DescendantTrivia();
            int commentOperatorCount = comments.Sum(comment => CountOccurrences(comment.ToString(), pattern));
            return CountOccurrences(method.ToString(), pattern) - commentOperatorCount;
        }

        private static int CountOccurrences(string text, string pattern)
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

        // Implementation based on https://github.com/mauricioaniche/ck
        private static int CountTryCatchBlocks(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<TryStatementSyntax>().Count();
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private static int CountLoops(MemberDeclarationSyntax method)
        {
            int count = method.DescendantNodes().OfType<ForEachStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<ForStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<WhileStatementSyntax>().Count();
            count += method.DescendantNodes().OfType<DoStatementSyntax>().Count();
            return count;
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private static int CountReturnStatements(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<ReturnStatementSyntax>().Count();
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private static int CountComparisonOperators(MemberDeclarationSyntax method)
        {
            int count = method.DescendantNodes().OfType<AssignmentExpressionSyntax>()
                .Count(n => n.IsKind(SyntaxKind.CoalesceAssignmentExpression));
            count += method.DescendantNodes().OfType<BinaryExpressionSyntax>()
                .Count(n => n.IsKind(SyntaxKind.EqualsExpression) || 
                            n.IsKind(SyntaxKind.NotEqualsExpression) ||
                            n.IsKind(SyntaxKind.LessThanExpression) ||
                            n.IsKind(SyntaxKind.LessThanOrEqualExpression) ||
                            n.IsKind(SyntaxKind.GreaterThanExpression) ||
                            n.IsKind(SyntaxKind.GreaterThanOrEqualExpression) ||
                            n.IsKind(SyntaxKind.CoalesceExpression));
            return count;
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private static int CountNumberOfAssignments(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<AssignmentExpressionSyntax>().Count();
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private static int CountNumberOfNumericLiterals(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<LiteralExpressionSyntax>().Count(n => n.IsKind(SyntaxKind.NumericLiteralExpression));
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private static int CountNumberOfStringLiterals(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<LiteralExpressionSyntax>().Count(n => n.IsKind(SyntaxKind.StringLiteralExpression));
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private static int CountNumberOfMathOperations(MemberDeclarationSyntax method)
        {
            int count = CountBinaryExpressions(method);
            count += CountUnaryExpressions(method);
            return count;
        }

        private static int CountBinaryExpressions(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<BinaryExpressionSyntax>()
                .Count(n => n.IsKind(SyntaxKind.AddExpression) ||
                            n.IsKind(SyntaxKind.SubtractExpression) ||
                            n.IsKind(SyntaxKind.MultiplyExpression) ||
                            n.IsKind(SyntaxKind.DivideExpression) ||
                            n.IsKind(SyntaxKind.ModuloExpression) ||
                            n.IsKind(SyntaxKind.LeftShiftExpression) ||
                            n.IsKind(SyntaxKind.RightShiftExpression));
        }

        private static int CountUnaryExpressions(MemberDeclarationSyntax method)
        {
            int count = method.DescendantNodes().OfType<PrefixUnaryExpressionSyntax>()
                .Count(n => n.IsKind(SyntaxKind.PreIncrementExpression) ||
                            n.IsKind(SyntaxKind.PreDecrementExpression) ||
                            n.IsKind(SyntaxKind.UnaryPlusExpression) ||
                            n.IsKind(SyntaxKind.UnaryMinusExpression));
            count += method.DescendantNodes().OfType<PostfixUnaryExpressionSyntax>()
                .Count(n => n.IsKind(SyntaxKind.PostIncrementExpression) ||
                            n.IsKind(SyntaxKind.PostDecrementExpression));
            return count;
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private static int CountNumberOfParenthesizedExpressions(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<ExpressionSyntax>()
                .Count(n => n.IsKind(SyntaxKind.ParenthesizedExpression));
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private static int CountNumberOfLambdaExpressions(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<LambdaExpressionSyntax>().Count();
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private static int CountMaxNestedBlocks(MemberDeclarationSyntax method)
        {
            var blocks = method.DescendantNodes().OfType<BlockSyntax>().ToList();

            List<int> blocksAncestors = new List<int>();
            foreach (var block in blocks)
            {
                blocksAncestors.Add(block.Ancestors().Count(a => a.IsKind(SyntaxKind.Block)));
            }

            if (!blocksAncestors.Any()) return 0;
            return blocksAncestors.Max();
        }

        // Implementation based on https://github.com/mauricioaniche/ck
        private static int CountNumberOfUniqueWords(MemberDeclarationSyntax method)
        {
            if (!method.Kind().Equals(SyntaxKind.MethodDeclaration)) return 0;

            BaseMethodDeclarationSyntax baseMethod = (BaseMethodDeclarationSyntax)method;
            string methodBody = RemoveCommentsFromCode(baseMethod.Body);
            methodBody = RemoveSymbols(methodBody);
            List<string> words = GetWords(methodBody);
            words = BreakWords(words);
            words = FilterWords(words);
            return words.Distinct().Count();
        }

        private static List<string> GetWords(string methodBody)
        {
            return Regex.Split(methodBody, "[\\s+]").Select(word => word.Trim()).ToList();
        }

        private static string RemoveSymbols(string words)
        {
            return words
                .Replace("(", " ")
                .Replace(")", " ")
                .Replace("{", " ")
                .Replace("}", " ")
                .Replace("=", " ")
                .Replace(">", " ")
                .Replace("<", " ")
                .Replace("&", " ")
                .Replace("|", " ")
                .Replace("!", " ")
                .Replace("+", " ")
                .Replace("*", " ")
                .Replace("/", " ")
                .Replace("-", " ")
                .Replace(";", " ")
                .Replace(":", " ")
                .Replace(",", " ")
                .Replace("[", " ")
                .Replace("]", " ")
                .Replace("\"", " ")
                .Replace(".", " ")
                .Replace("?", " ");
        }

        private static List<string> FilterWords(List<string> words)
        {
            return words = words.Where(word => !string.IsNullOrEmpty(word))
                .Where(word => !Regex.IsMatch(word, "[0-9]+"))
                .Where(word => !GetKeywords().Contains(word))
                .ToList();
        }

        private static List<string> BreakWords(List<string> words)
        {
            List<string> individualWords = new List<string>();
            foreach (string word in words)
            {
                List<string> wordParts = Regex.Split(word, "[_ ]").ToList();
                List<string> camelCaseWords = GetCamelCaseWords(wordParts);
                individualWords.AddRange(camelCaseWords);
            }
            return individualWords;
        }

        private static List<string> GetCamelCaseWords(List<string> words)
        {
            List<string> camelCaseWords = new List<string>();
            foreach (string word in words)
            {
                var wordParts = Regex.Split(word, "[A-Z]");
                var matches = Regex.Matches(word, "[A-Z]");
                for (int i = 0; i < wordParts.Length - 1; i++)
                {
                    wordParts[i + 1] = matches[i] + wordParts[i + 1];
                }
                camelCaseWords.AddRange(wordParts);
            }

            return camelCaseWords;
        }

        private static List<string> GetKeywords()
        {
            List<string> keywords = new List<string>();
            keywords.Add("abstract");
            keywords.Add("as");
            keywords.Add("base");
            keywords.Add("bool");
            keywords.Add("break");
            keywords.Add("byte");
            keywords.Add("case");
            keywords.Add("catch");
            keywords.Add("char");
            keywords.Add("checked");
            keywords.Add("class");
            keywords.Add("const");
            keywords.Add("continue");
            keywords.Add("decimal");
            keywords.Add("default");
            keywords.Add("delegate");
            keywords.Add("do");
            keywords.Add("double");
            keywords.Add("else");
            keywords.Add("enum");
            keywords.Add("event");
            keywords.Add("explicit");
            keywords.Add("extern");
            keywords.Add("false");
            keywords.Add("finally");
            keywords.Add("fixed");
            keywords.Add("float");
            keywords.Add("for");
            keywords.Add("foreach");
            keywords.Add("goto");
            keywords.Add("if");
            keywords.Add("implicit");
            keywords.Add("in");
            keywords.Add("int");
            keywords.Add("interface");
            keywords.Add("internal");
            keywords.Add("is");
            keywords.Add("lock");
            keywords.Add("long");
            keywords.Add("namespace");
            keywords.Add("new");
            keywords.Add("null");
            keywords.Add("object");
            keywords.Add("operator");
            keywords.Add("out");
            keywords.Add("override");
            keywords.Add("params");
            keywords.Add("private");
            keywords.Add("protected");
            keywords.Add("public");
            keywords.Add("readonly");
            keywords.Add("record");
            keywords.Add("ref");
            keywords.Add("return");
            keywords.Add("sbyte");
            keywords.Add("sealed");
            keywords.Add("short");
            keywords.Add("sizeof");
            keywords.Add("stackalloc");
            keywords.Add("static");
            keywords.Add("string");
            keywords.Add("struct");
            keywords.Add("switch");
            keywords.Add("this");
            keywords.Add("throw");
            keywords.Add("true");
            keywords.Add("try");
            keywords.Add("typeof");
            keywords.Add("uint");
            keywords.Add("ulong");
            keywords.Add("unchecked");
            keywords.Add("unsafe");
            keywords.Add("ushort");
            keywords.Add("using");
            keywords.Add("virtual");
            keywords.Add("void");
            keywords.Add("volatile");
            keywords.Add("while");
            return keywords;
        }

    }
}