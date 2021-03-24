using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
                NONL = CountNumberOfNumericLiterals(member),
                NOSL = CountNumberOfStringLiterals(member),
                NOMO = CountNumberOfMathOperations(member),
                NOPE = CountNumberOfParenthesizedExpressions(member),
                NOLE = CountNumberOfLambdaExpressions(member),
                MNB = CountMaxNestedBlocks(member),
                NOUW = CountNumberOfUniqueWords(member)
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

        private int CountNumberOfNumericLiterals(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<LiteralExpressionSyntax>().Count(n => n.IsKind(SyntaxKind.NumericLiteralExpression));
        }

        private int CountNumberOfStringLiterals(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<LiteralExpressionSyntax>().Count(n => n.IsKind(SyntaxKind.StringLiteralExpression));
        }

        private int CountNumberOfMathOperations(MemberDeclarationSyntax method)
        {
            int count = CountBinaryExpressions(method);
            count += CountUnaryExpressions(method);
            return count;
        }

        private int CountBinaryExpressions(MemberDeclarationSyntax method)
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

        private int CountUnaryExpressions(MemberDeclarationSyntax method)
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

        private int CountNumberOfParenthesizedExpressions(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<ExpressionSyntax>()
                .Count(n => n.IsKind(SyntaxKind.ParenthesizedExpression));
        }

        private int CountNumberOfLambdaExpressions(MemberDeclarationSyntax method)
        {
            return method.DescendantNodes().OfType<LambdaExpressionSyntax>().Count();
        }

        private int CountMaxNestedBlocks(MemberDeclarationSyntax method)
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

        private int CountNumberOfUniqueWords(MemberDeclarationSyntax method)
        {
            if (!method.Kind().Equals(SyntaxKind.MethodDeclaration)) return 0;
            
            BaseMethodDeclarationSyntax baseMethod = (BaseMethodDeclarationSyntax)method;
            string methodBody = baseMethod.Body.ToString();
            List<string> lines = GetLinesWithoutComments(methodBody);
            List<string> words = GetWordsFromLines(lines);
            words = FilterWords(words);
            List<string> individualWords = BreakWords(words);
            individualWords = FindAdditionalWords(individualWords);
            return CleanWordsFromSymbols(individualWords).Distinct().ToList().Count();
        }

        private List<string> GetLinesWithoutComments(string methodBody)
        {
            List<string> lines = methodBody.Split("\n")
                .Select(line => line.Trim())
                .Where(line => !line.StartsWith("//"))
                .ToList();
            return RemoveMultiLineComments(lines);
        }

        private List<string> RemoveMultiLineComments(List<string> lines)
        {
            List<string> linesToRemove = new List<string>();
            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i].StartsWith("/*"))
                {
                    linesToRemove.Add(lines[i]);
                    int j = 1;
                    while (!lines[i + j].EndsWith("*/"))
                    {
                        linesToRemove.Add(lines[i + j]);
                        j++;
                    }
                    linesToRemove.Add(lines[i + j]);
                }
            }
            lines.RemoveAll(line => linesToRemove.Contains(line));
            return lines;
        }

        private List<string> GetWordsFromLines(List<string> lines)
        {
            List<string> words = new List<string>();

            foreach (string line in lines)
            {
                int index = 0;
                while (index < line.Length && char.IsWhiteSpace(line[index]))
                    index++;

                int start;
                while (index < line.Length)
                {
                    start = index;
                    while (index < line.Length && !char.IsWhiteSpace(line[index]))
                        index++;

                    words.Add(line.Substring(start, index - start));

                    while (index < line.Length && char.IsWhiteSpace(line[index]))
                        index++;
                }
            }
            return words;
        }

        private List<string> FilterWords(List<string> words)
        {
            for (int i = 0; i < words.Count(); i++)
            {
                words[i] = words[i]
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
                    .Replace("\"", " ");
            }
            words = words.Where(word => !string.IsNullOrEmpty(word))
                .Where(word => !Regex.IsMatch(word, "[0-9]+"))
                .Where(word => !getKeywords().Contains(word))
                .ToList();
            return words;
        }

        private List<string> BreakWords(List<string> words)
        {
            List<string> individualWords = new List<string>();
            foreach (string word in words)
            {
                if (word.Length == 1)
                {
                    individualWords.Add(word.Trim());

                }
                else
                {
                    if (getKeywords().Contains(word.Trim()))
                    {
                        continue;
                    }

                    int current = 0;
                    for (int i = 1; i < word.Length; i++)
                    {
                        if (word[i] == '_' || char.IsUpper(word[i]))
                        {
                            individualWords.Add(word.Substring(current, i - current).Trim());
                            current = i + (word[i] == '_' ? 1 : 0);
                        }
                    }
                    individualWords.Add(word.Substring(current).Trim());
                }
            }
            return individualWords.Where(word => !string.IsNullOrEmpty(word)).ToList();
        }

        private List<string> FindAdditionalWords(List<string> words)
        {
            List<string> result = new List<string>();
            foreach (string word in words)
            {
                if (word.Contains(" "))
                {
                    foreach (string w in word.Split(" "))
                    {
                        result.Add(w);
                    }
                }
                else
                {
                    result.Add(word);
                }
            }
            return result;
        }

        private List<string> CleanWordsFromSymbols(List<string> words)
        {
            List<string> cleanWords = new List<string>();
            foreach (string word in words)
            {
                if (word.Length > 1)
                {
                    if (!char.IsLetter(word[0]))
                    {
                        cleanWords.Add(word.Substring(1));
                    }
                    else if (!char.IsLetter(word[word.Length - 1]))
                        cleanWords.Add(word.Substring(0, word.Length - 2));
                    else
                    {
                        cleanWords.Add(word);
                    }

                }
                else
                {
                    cleanWords.Add(word);
                }
            }
            return cleanWords;
        }

        private List<string> getKeywords()
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