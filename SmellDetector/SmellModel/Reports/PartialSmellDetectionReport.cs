using System.Collections.Generic;

namespace SmellDetector.SmellModel.Reports
{
    public class PartialSmellDetectionReport
    {
        public Dictionary<string, ISet<Issue>> CodeSnippetIssues { get; set; }

        public PartialSmellDetectionReport()
        {
            CodeSnippetIssues = new Dictionary<string, ISet<Issue>>();
        }

        public void AddIssue(string codeItemId, Issue issue)
        {
            if (!CodeSnippetIssues.ContainsKey(codeItemId)) CodeSnippetIssues.Add(codeItemId, new HashSet<Issue> { issue });
            else CodeSnippetIssues[codeItemId].Add(issue);
        }
    }
}
