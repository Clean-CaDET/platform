using SmellDetector.Controllers;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;
using System;

namespace SmellDetector.Detectors
{
    internal class GodClassRuleEngine : IDetector
    {
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
            if (metrics.WMC > 47 && metrics.ATFD > 5 && metrics.TCC < 0.33) return true;
            if (metrics.LOC > 100) return true;
            if ((metrics.NMD + metrics.NAD) > 20) return true;

            return false;
        }
    }
}