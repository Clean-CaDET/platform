using System;
using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.SourceCode
{
    public class BracketsChecker
    {
        private readonly char[] openBrackets = { '(', '[', '{' };
        private readonly char[] closeBrackets = { ')', ']', '}' };

        public bool BracketsMatching(string[] sourceCode)
        {
            Stack<int> brackets = new Stack<int>();
            foreach (var line in sourceCode)
            {
                foreach (char bracket in GetCodeWithoutSummary(line).ToCharArray())
                {
                    int index;
                    if ((index = Array.IndexOf(openBrackets, bracket)) != -1)
                        brackets.Push(index);
                    else if ((index = Array.IndexOf(closeBrackets, bracket)) != -1)
                        if (brackets.Count == 0 || brackets.Pop() != index)
                            return false;

                }
            }
            return brackets.Count == 0;
        }

        private string GetCodeWithoutSummary(string code)
        {
            int summaryIndex = code.IndexOf("<summary>");
            if (summaryIndex != -1)
            {
                string codeBeforeComment = code.Substring(0, summaryIndex);
                string codeAfterComment = code.Substring(code.IndexOf("</summary>") + "</summary>".Length);
                code = codeBeforeComment + codeAfterComment;
            }
            return code;
        }
    }
}
