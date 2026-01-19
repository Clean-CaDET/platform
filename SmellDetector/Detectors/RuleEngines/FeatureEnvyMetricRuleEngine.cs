using CodeModel.CaDETModel.CodeItems;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SmellDetector.Detectors.RuleEngines
{
    public class FeatureEnvyMetricRuleEngine : IDetector
    {
        private readonly List<Rule> _rules;
        public FeatureEnvyMetricRuleEngine()
        {
            Rule rule1 = new Rule("10.1109/WCRE.2005.15",
                                    new OrCriteria(
                                        new MetricCriteria(CaDETMetric.CBO, OperationEnum.GREATER_THAN, 5),
                                        new MetricCriteria(CaDETMetric.LCOM, OperationEnum.GREATER_THAN, 2)),
                                  SmellType.FEATURE_ENVY);
            Rule rule2 = new Rule("",
                                    new MetricCriteria(CaDETMetric.LCOM, OperationEnum.GREATER_THAN, 0.725),
                                    SmellType.FEATURE_ENVY);
          
            _rules = new List<Rule>();
            _rules.Add(rule1);
            _rules.Add(rule2);
        }

        public PartialSmellDetectionReport FindIssues(List<CaDETClass> classes)
        {
            List<CaDETMember> methods = classes.SelectMany(c => c.Members).ToList();
            var partialReport = new PartialSmellDetectionReport();

            foreach (var method in methods)
            {
                var issues = ApplyRules(method);
                foreach (var issue in issues.Where(issue => issue != null))
                {
                    partialReport.AddIssue(issue.CodeSnippetId, issue);
                }
            }
            return partialReport;
        }

        private List<Issue> ApplyRules(CaDETMember m)
        {
            List<Issue> issues = _rules.Select(r => r.Validate(m.Name, m.Metrics)).ToList();
            return issues;
        }
    }
}
