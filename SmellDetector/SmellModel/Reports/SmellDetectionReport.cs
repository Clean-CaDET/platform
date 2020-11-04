using System.Collections.Generic;

namespace SmellDetector.SmellModel.Reports
{
    public class SmellDetectionReport
    {
        public Dictionary<string, List<Issue>> Report { get; set; }

        public SmellDetectionReport()
        {
            Report = new Dictionary<string, List<Issue>>();
        }

        public void AddPartialReport(PartialSmellDetectionReport partialReport)
        {
            foreach (var codeItemIssue in partialReport.CodeItemIssues)
            {
                if (Report.ContainsKey(codeItemIssue.Key))
                {
                    Report[codeItemIssue.Key].AddRange(codeItemIssue.Value);
                }
                else
                {
                    Report[codeItemIssue.Key] = new List<Issue>();
                    Report[codeItemIssue.Key].AddRange(codeItemIssue.Value);
                }

            }
        }

    }
}
