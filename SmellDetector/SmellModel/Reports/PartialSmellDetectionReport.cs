using System.Collections.Generic;

namespace SmellDetector.SmellModel.Reports
{
    public class PartialSmellDetectionReport
    {
        public Dictionary<string, List<Issue>> PartialReport { get; set; }

        public PartialSmellDetectionReport()
        {
            PartialReport = new Dictionary<string, List<Issue>>();
        }

        public void AddIssue(string codeItemId, Issue issue) { AddIssues(codeItemId, new List<Issue> { issue }); }

        public void AddIssues(string codeItemId, List<Issue> issues)
        {
            if (!PartialReport.ContainsKey(codeItemId)) PartialReport.Add(codeItemId, issues);
            else PartialReport[codeItemId].AddRange(issues);
        }
    }
}
