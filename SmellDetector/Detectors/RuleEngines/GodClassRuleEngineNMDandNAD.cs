using System;
using SmellDetector.Controllers;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;

namespace SmellDetector.Detectors.RuleEngines
{
    internal class GodClassRuleEngineNMDandNAD : IDetector
    {
        // Kiefer C, Bernstein A, Tappolet J. Mining software repositories with isparol and a software evolution ontology. In Proceedings of the Fourth International Workshop on Mining Software Repositories. IEEE Computer Society; 2007:1-8.
        private int nmdThreshold;
        private int nadThreshold;
        public GodClassRuleEngineNMDandNAD(int nmdThreshold, int nadThreshold)
        {
            this.nmdThreshold = nmdThreshold;
            this.nadThreshold = nadThreshold;
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
            if (metrics.NMD > this.nmdThreshold) return true;
            if (metrics.NAD > this.nadThreshold) return true;
            return false;
        }
    }
}
