using SmellDetector.Controllers;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;

namespace SmellDetector.Detectors.RuleEngines
{
    public class LongParameterListRuleEngine : IDetector
    {
        public PartialSmellDetectionReport FindIssues(CaDETClassDTO caDetClassDto)
        {
            PartialSmellDetectionReport partialReport = new PartialSmellDetectionReport();

            foreach (var identifierAnalysis in caDetClassDto.IdentifierAnalyses)
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

        public bool IsBadSmell(MetricsDTO metrics)
        {
            return metrics.NOP > 5;
        }
    }
}
