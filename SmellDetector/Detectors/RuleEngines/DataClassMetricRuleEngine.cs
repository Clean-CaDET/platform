using CodeModel.CaDETModel.CodeItems;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;
using System.Collections.Generic;
using System.Linq;
using System;
namespace SmellDetector.Detectors.RuleEngines
{
    internal class DataClassMetricRuleEngine : IDetector
    {
        private readonly List<Rule> _rules;
        public DataClassMetricRuleEngine()
        {
            Rule rule1 = new Rule("10.1109/WCRE.2005.15",
                                  new AndCriteria(
                                        new AndCriteria(
                                                new AndCriteria(
                                                    new MetricCriteria(CaDETMetric.WOC, OperationEnum.LESS_THAN, 0.33),
                                                    new MetricCriteria(CaDETMetric.NOPA_NOPP, OperationEnum.GREATER_THAN, 4)),
                                                new OrCriteria(
                                                    new MetricCriteria(CaDETMetric.WMC, OperationEnum.LESS_THAN, 47),
                                                    new MetricCriteria(CaDETMetric.NOPA_NOPP, OperationEnum.GREATER_THAN, 2))
                                                ),
                                                new MetricCriteria(CaDETMetric.WMC, OperationEnum.LESS_THAN, 31)),
                                  SmellType.DATA_CLASS);
            Rule rule2 = new Rule("",
                                   new OrCriteria(
                                            new MetricCriteria(CaDETMetric.LCOM, OperationEnum.GREATER_THAN, 2),
                                            new MetricCriteria(CaDETMetric.NOPP, OperationEnum.GREATER_THAN, 10)
                                       ),
                                    SmellType.DATA_CLASS);
            Rule rule3 = new Rule("",
                                   new OrCriteria(
                                            new MetricCriteria(CaDETMetric.WMC, OperationEnum.LESS_THAN, 50),
                                            new MetricCriteria(CaDETMetric.LCOM, OperationEnum.LESS_THAN, 0.8)
                                       ),
                                    SmellType.DATA_CLASS);
            Rule rule4 = new Rule("",
                                   new OrCriteria(
                                       new OrCriteria(
                                                new MetricCriteria(CaDETMetric.WOC, OperationEnum.LESS_THAN, 0.33),
                                                new MetricCriteria(CaDETMetric.NOPA, OperationEnum.GREATER_THAN, 3)
                                           ),
                                       new MetricCriteria(CaDETMetric.NOPP, OperationEnum.GREATER_THAN, 3)),
                                    SmellType.DATA_CLASS);
            _rules = new List<Rule>();
            _rules.Add(rule1);
            _rules.Add(rule2);
            _rules.Add(rule3);
            _rules.Add(rule4);
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
