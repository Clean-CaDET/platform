using System.Collections.Generic;
using SmellDetector.Controllers;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;

namespace SmellDetector.Detectors.RuleEngines
{
    public class LongParameterListRuleEngine : IDetector
    {
        public PartialSmellDetectionReport FindIssues(CodeSnippetCollectionDTO codeSnippetCollectionDTO)
        {
            PartialSmellDetectionReport partialReport = new PartialSmellDetectionReport();

            foreach (var identifierAnalysis in codeSnippetCollectionDTO.CodeItemMetrics)
            {
                if (IsBadSmell(identifierAnalysis.Value))
                {
                    Issue newIssue = new Issue();
                    newIssue.IssueType = SmellType.LONG_PARAM_LISTS;
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

        public bool IsBadSmell(MetricsDTO metrics)
        {
            return metrics.NOP > 5;
        }
    }
}
