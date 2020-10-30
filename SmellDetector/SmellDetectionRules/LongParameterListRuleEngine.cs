using SmellDetector.Controllers;
using SmellDetector.SmellModel;

namespace SmellDetector.SmellDetectionRules
{
    public class LongParameterListRuleEngine: IDetector, RuleEngine
    {
        public PartialSmellDetectionReport findIssues(CaDETClassDTO caDetClassDto)
        {
            PartialSmellDetectionReport partialReport = new PartialSmellDetectionReport();

            foreach (var pair in caDetClassDto.CaDETClass)
            {
                Issue newIssue = new Issue();
                if (isBadSmell(pair.Value))
                {
                    newIssue.IssueType = SmellType.LONG_PARAM_LISTS;
                    newIssue.Problem += "Long parameter list in " + pair.Key;
                }
                else
                {
                    newIssue.IssueType = SmellType.WITHOUT_BAD_SMELL;
                    newIssue.Problem += "Without problem in " + pair.Key;
                }
                partialReport.AddIssue(pair.Key, newIssue);
            }

            return null;
        }

        public bool isBadSmell(MetricsDTO metrics)
        {
            if (metrics.NOP > 5) return true;
            return false;
        }
    }
}
