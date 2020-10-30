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

            foreach (var pair in caDetClassDto.CaDETClass)
            {
                Issue newIssue = new Issue();
                if (IsBadSmell(pair.Value)) newIssue.IssueType = SmellType.LONG_PARAM_LISTS;
                newIssue.CodeItemId = pair.Key;
                partialReport.AddIssue(pair.Key, newIssue);
            }

            return null;
        }

        public bool IsBadSmell(MetricsDTO metrics)
        {
            return metrics.NOP > 5;
        }
    }
}
