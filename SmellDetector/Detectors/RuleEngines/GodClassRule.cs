using SmellDetector.Controllers;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;
using System;
using System.Collections.Generic;

namespace SmellDetector.Detectors
{

    // Trifu A, Marinescu R. Diagnosing design problems in object oriented systems. In Proceedings of the 12th Working Conference on Reverse Engineering. IEEE; 2005:1-10.
    // Schumacher J, Zazworka N, Shull F, Seaman C, Shaw M. Building empirical support for automated code smell detection. In Proceedings of the International Symposium on Empirical Software Engineering and Measurement. ACM; 2010:8-18
    internal class GodClassRule : IDetector
    {
        private readonly double _wmcThreshold;
        private readonly double _atfdThreshold;
        private readonly double _tccThreshold;

        public GodClassRule(double wmcThreshold, double atfdThreshold, double tccThreshold)
        {
            _wmcThreshold = wmcThreshold;
            _atfdThreshold = atfdThreshold;
            _tccThreshold = tccThreshold;
        }

        public PartialSmellDetectionReport FindIssues(CodeSnippetCollectionDTO codeSnippetCollectionDTO)
        {
            PartialSmellDetectionReport partialReport = new PartialSmellDetectionReport();

            foreach (var identifierAnalysis in codeSnippetCollectionDTO.CodeItemMetrics)
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

        public PartialSmellDetectionReport FindIssues(List<CaDETClassDTO> caDetClassDto)
        {
            throw new NotImplementedException();
        }

        private bool IsBadSmell(MetricsDTO metrics)
        {
            if (metrics.WMC > _wmcThreshold && metrics.ATFD > _atfdThreshold && metrics.TCC < _tccThreshold) return true;
            return false;
        }
    }
}