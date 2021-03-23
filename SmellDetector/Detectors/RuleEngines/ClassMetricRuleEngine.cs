using System;
using System.Collections.Generic;
using System.Linq;
using SmellDetector.Controllers;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;

namespace SmellDetector.Detectors.RuleEngines
{
    internal class ClassMetricRuleEngine : IDetector
    {
        private readonly List<Rule> _rules;
        public ClassMetricRuleEngine(List<Rule> rules)
        {
            _rules = rules;
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

        private List<Issue> ApplyRule(CaDETClassDTO c)
        {
            return (List<Issue>)_rules.Select(r => r.IsTriggered("", new Dictionary<string, double>()));
        }

        private bool IsBadSmell(MetricsDTO value)
        {
            //bool isBadSmell = false;
            //foreach(Criteria criteria in criterias.Values)
            //{
            //    isBadSmell = criteria.MeetCriteria();
            //}
            //return isBadSmell;
            return false;
        }
    }
}
