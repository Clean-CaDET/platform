using System.Collections.Generic;

namespace SmellDetector.SmellModel.Reports
{
    public class SmellDetectionReport
    {
        public Dictionary<string, List<Issue>> IssuesForCodeSnippet { get; set; }

        public SmellDetectionReport()
        {
            IssuesForCodeSnippet = new Dictionary<string, List<Issue>>();
        }

        public void AddPartialReport(PartialSmellDetectionReport partialReport)
        {
            foreach (var codeItemIssue in partialReport.CodeItemIssues)
            {
                if (IssuesForCodeSnippet.ContainsKey(codeItemIssue.Key))
                {
                    IssuesForCodeSnippet[codeItemIssue.Key].AddRange(codeItemIssue.Value);
                }
                else
                {
                    IssuesForCodeSnippet[codeItemIssue.Key] = new List<Issue>();
                    IssuesForCodeSnippet[codeItemIssue.Key].AddRange(codeItemIssue.Value);
                }

            }
        }

    }
}
