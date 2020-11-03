using System.Collections.Generic;

namespace SmellDetector.SmellModel.Reports
{
    public class PartialSmellDetectionReport
    {
        public Dictionary<string, List<Issue>> CodeItemIssues { get; set; }

        public PartialSmellDetectionReport()
        {
            CodeItemIssues = new Dictionary<string, List<Issue>>();
        }

        public void AddIssue(string codeItemId, Issue issue) { AddIssues(codeItemId, new List<Issue> { issue }); }

        public void AddIssues(string codeItemId, List<Issue> issues)
        {
            if (!CodeItemIssues.ContainsKey(codeItemId)) CodeItemIssues.Add(codeItemId, issues);
            else CodeItemIssues[codeItemId].AddRange(issues);
        }
    }
}
