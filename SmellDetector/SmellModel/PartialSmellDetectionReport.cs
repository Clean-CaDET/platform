using System.Collections.Generic;

namespace SmellDetector.SmellModel
{
    public class PartialSmellDetectionReport
    {
        public Dictionary<string, List<Issue>> PartialReport { get; set; }

        public PartialSmellDetectionReport()
        {
            PartialReport = new Dictionary<string, List<Issue>>();
        }

        public void AddIssue(string id, Issue issue)
        {
            if (!PartialReport.ContainsKey(id)) PartialReport.Add(id, new List<Issue> { issue });
            else PartialReport[id].Add(issue);
        }

        public void AddIssues(string id, List<Issue> issues)
        {
            if (!PartialReport.ContainsKey(id)) PartialReport.Add(id, issues);
            else PartialReport[id].AddRange(issues);
        }
    }
}
