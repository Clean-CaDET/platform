using System;
using System.Collections.Generic;
using SmellDetector.Controllers;
using SmellDetector.SmellModel;

namespace SmellDetector.SmellDetectionRules
{
    public class LongMethodRuleEngine : IDetector, RuleEngine
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
                    newIssue.Problem += "Long method in " + pair.Key;
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
            if (metrics.LOC > 50) return true;
            if (metrics.CYCLO > 5) return true;
            if (metrics.NOP > 4 || metrics.NOLV > 4) return true;

            return false;
        }
    }
}
