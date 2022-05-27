using CodeModel.CaDETModel.CodeItems;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;
using System.Collections.Generic;
using System.Linq;
using System;
namespace SmellDetector.Detectors.RuleEngines
{
    public class RefuseBequestMetricRuleEngine : IDetector
    {
        private readonly List<Rule> _rules;
        public RefuseBequestMetricRuleEngine()
        {
            Rule rule1 = new Rule("",
                                   new AndCriteria(
                                            new MetricCriteria(CaDETMetric.BUR, OperationEnum.LESS_THAN, 1/3),
                                            new MetricCriteria(CaDETMetric.NMD_NAD, OperationEnum.GREATER_THAN, 2)
                                       ),
                                    SmellType.REFUSE_BEQUEST);
            _rules = new List<Rule>();
            _rules.Add(rule1);
        }

        public PartialSmellDetectionReport FindIssues(List<CaDETClass> classes)
        {
            var partialReport = new PartialSmellDetectionReport();

            foreach (var cadetClass in classes)
            {
                var issues = ApplyRules(cadetClass);
                foreach (var issue in issues.Where(issue => issue != null))
                {
                    partialReport.AddIssue(issue.CodeSnippetId, issue);
                }
            }
            return partialReport;
        }

        private List<Issue> ApplyRules(CaDETClass c)
        {
            List<Issue> issues = _rules.Select(r => r.Validate(c.FullName, c.Metrics)).ToList();
            return issues;
        }
    }
}
