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
                Report[pair.Key].AddRange(pair.Value);
            }
        }

    }
}
