using System.Collections.Generic;

namespace SmellDetector.SmellModel.Reports
{
    public class SmellDetectionReport
    {
        public Dictionary<string,List<Issue>> Report { get; set; }

        public SmellDetectionReport()
        {
            Report = new Dictionary<string, List<Issue>>();
        }

        public void AddPartialReport(PartialSmellDetectionReport partialReport)
        {
            foreach (var pair in partialReport.PartialReport)
            {
                if (Report.ContainsKey(pair.Key))
                {
                    Report[pair.Key].AddRange(pair.Value);
                }
                else
                {
                    Report[pair.Key] = new List<Issue>();
                    Report[pair.Key].AddRange(pair.Value);
                }

            }
        }

    }
}
