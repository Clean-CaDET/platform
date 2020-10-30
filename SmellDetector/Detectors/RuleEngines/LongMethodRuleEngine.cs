using SmellDetector.Controllers;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;

namespace SmellDetector.Detectors.RuleEngines
{
    public class LongMethodRuleEngine : IDetector
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

        private bool IsBadSmell(MetricsDTO metrics)
        {
            if (metrics.LOC > 50) return true;
            if (metrics.CYCLO > 5) return true;
            if (metrics.NOP > 4 || metrics.NOLV > 4) return true;

            return false;
        }
    }
}
