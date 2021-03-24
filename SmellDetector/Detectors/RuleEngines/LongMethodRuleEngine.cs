using System.Collections.Generic;
using SmellDetector.Controllers;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;

namespace SmellDetector.Detectors.RuleEngines
{
    public class LongMethodRuleEngine : IDetector
    {

        public PartialSmellDetectionReport FindIssues(CodeSnippetCollectionDTO caDetClassDto)
        {
            PartialSmellDetectionReport partialReport = new PartialSmellDetectionReport();

            foreach (var identifierAnalysis in caDetClassDto.CodeItemMetrics)
            {
                
                if (IsBadSmell(identifierAnalysis.Value))
                {
                    Issue newIssue = new Issue();
                    newIssue.IssueType = SmellType.LONG_METHOD;
                    newIssue.CodeItemId = identifierAnalysis.Key;
                    partialReport.AddIssue(identifierAnalysis.Key, newIssue);
                }
                
            }

            return partialReport;
        }

        public PartialSmellDetectionReport FindIssues(List<CaDETClassDTO> caDetClassDto)
        {
            throw new System.NotImplementedException();
        }

        private bool IsBadSmell(MetricsDTO metrics)
        {
            if (metrics.LOC > 50) return true;
            if (metrics.CYCLO > 5) return true;
            if (metrics.NOP > 4 || metrics.NOLV > 4) return true;

            return false;
        }
    }
}
