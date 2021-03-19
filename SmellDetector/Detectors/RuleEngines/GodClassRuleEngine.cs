using SmellDetector.Controllers;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;
using System;

namespace SmellDetector.Detectors
{

    // Trifu A, Marinescu R. Diagnosing design problems in object oriented systems. In Proceedings of the 12th Working Conference on Reverse Engineering. IEEE; 2005:1-10.
    // Schumacher J, Zazworka N, Shull F, Seaman C, Shaw M. Building empirical support for automated code smell detection. In Proceedings of the International Symposium on Empirical Software Engineering and Measurement. ACM; 2010:8-18
    internal class GodClassRuleEngine : IDetector
    {
        private double wmcThreshold;
        private double atfdThreshold;
        private double tccThreshold;

        public GodClassRuleEngine(double wmcThreshold, double atfdThreshold, double tccThreshold)
        {
            this.wmcThreshold = wmcThreshold;
            this.atfdThreshold = atfdThreshold;
            this.tccThreshold = tccThreshold;
        }

        public PartialSmellDetectionReport FindIssues(CaDETClassDTO caDetClassDto)
        {
            PartialSmellDetectionReport partialReport = new PartialSmellDetectionReport();

            foreach (var identifierAnalysis in caDetClassDto.CodeItemMetrics)
            {

                if (IsBadSmell(identifierAnalysis.Value))
                {
                    Issue newIssue = new Issue();
                    newIssue.IssueType = SmellType.GOD_CLASS;
                    newIssue.CodeItemId = identifierAnalysis.Key;
                    partialReport.AddIssue(identifierAnalysis.Key, newIssue);
                }

            }

            return partialReport;
        }

        private bool IsBadSmell(MetricsDTO metrics)
        {
            if (metrics.WMC > this.wmcThreshold && metrics.ATFD > this.atfdThreshold && metrics.TCC < this.tccThreshold) return true;
            return false;
        }
    }
}