using System;
using System.Collections.Generic;
using SmellDetector.Controllers;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;

namespace SmellDetector.Detectors.RuleEngines
{
    internal class GodClassRuleNMDandNAD : IDetector
    {
        // Kiefer C, Bernstein A, Tappolet J. Mining software repositories with isparol and a software evolution ontology. In Proceedings of the Fourth International Workshop on Mining Software Repositories. IEEE Computer Society; 2007:1-8.
        private readonly int _nmdThreshold;
        private readonly int _nadThreshold;
        public GodClassRuleNMDandNAD(int nmdThreshold, int nadThreshold)
        {
            _nmdThreshold = nmdThreshold;
            _nadThreshold = nadThreshold;
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
            if (metrics.NMD > _nmdThreshold) return true;
            if (metrics.NAD > _nadThreshold) return true;
            return false;
        }
    }
}
