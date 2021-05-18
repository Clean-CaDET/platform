using System.Collections.Generic;

namespace SmellDetector.SmellModel.Reports
{
    public class SmellDetectionReport
    {
        public Dictionary<string, ISet<Issue>> IssuesForCodeSnippet { get; set; }

        public SmellDetectionReport()
        {
            IssuesForCodeSnippet = new Dictionary<string, ISet<Issue>>();
        }

        public void AddPartialReport(PartialSmellDetectionReport partialReport)
        {
            foreach (var codeItemIssue in partialReport.CodeSnippetIssues)
            {
                if (IssuesForCodeSnippet.ContainsKey(codeItemIssue.Key))
                {
                    IssuesForCodeSnippet[codeItemIssue.Key].UnionWith(codeItemIssue.Value);
                }
                else
                {
                    IssuesForCodeSnippet[codeItemIssue.Key] = new HashSet<Issue>();
                    IssuesForCodeSnippet[codeItemIssue.Key].UnionWith(codeItemIssue.Value);
                }

            }
        }

    }
}
