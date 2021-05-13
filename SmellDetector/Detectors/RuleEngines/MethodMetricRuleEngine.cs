using System;
using System.Collections.Generic;
using System.Linq;
using CodeModel.CaDETModel.CodeItems;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;

namespace SmellDetector.Detectors.RuleEngines
{
    public class MethodMetricRuleEngine : IDetector
    {

        private readonly List<Rule> _rules;
        private readonly List<Rule> _dynamicRules;

        public MethodMetricRuleEngine()
        {
            _rules = new List<Rule>();
            _dynamicRules = new List<Rule>();
            Rule rule1 = new Rule("10.1109/SCAM.2013.6648192",
                                  new MetricCriteria(CaDETMetric.MLOC, OperationEnum.GREATER_THAN, 50),
                                  SmellType.LONG_METHOD);
            Rule rule2 = new Rule("10.1016/j.jss.2006.10.018",
                                  new OrCriteria(
                                      new MetricCriteria(CaDETMetric.MLOC, OperationEnum.GREATER_THAN, 50),
                                      new MetricCriteria(CaDETMetric.CYCLO, OperationEnum.GREATER_THAN, 10)),
                                  SmellType.LONG_METHOD);
            Rule rule3 = new Rule("https://doi.org/10.1145/3132498.3134268",
                                  new AndCriteria(
                                      new AndCriteria(
                                          new MetricCriteria(CaDETMetric.MLOC, OperationEnum.GREATER_THAN, 30),
                                          new MetricCriteria(CaDETMetric.CYCLO, OperationEnum.GREATER_THAN, 4)),
                                      new MetricCriteria(CaDETMetric.MMNB, OperationEnum.GREATER_THAN, 3)),
                                  SmellType.LONG_METHOD);
        }

        public PartialSmellDetectionReport FindIssues(List<CaDETClass> classes)
        {
            List<CaDETMember> methods = classes.SelectMany(c => c.Members).ToList();

            var partialReport = new PartialSmellDetectionReport();

            foreach (var method in methods)
            {
                var issues = ApplyRule(method);
                foreach (var issue in issues.Where(issue => issue != null))
                {
                    partialReport.AddIssue(issue.CodeSnippetId, issue);
                }
            }
            return partialReport;
        }

        private List<Issue> ApplyRule(CaDETMember m)
        {
            List<Issue> issues = _rules.Select(r => r.Validate(m.Name, m.Metrics)).ToList();
            issues.AddRange(_dynamicRules.Select(r => r.Validate(m.Name, m.Metrics)).ToList());
            return issues;
        }
    }
}
