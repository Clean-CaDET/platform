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
        public ClassMetricRuleEngine()
        {
            Rule rule1 = new Rule("10.1109/WCRE.2005.15",
                                  new AndCriteria(
                                            new AndCriteria(
                                                new MetricCriteria("ATFD", OperationEnum.GREATER_THAN, 2),
                                                new MetricCriteria("WMC", OperationEnum.GREATER_THAN, 47)),
                                            new MetricCriteria("TCC", OperationEnum.LESS_THAN, 0.33)),
                                  SmellType.GOD_CLASS);
            Rule rule2 = new Rule("10.1109/SCAM.2013.6648192",
                                  new OrCriteria(
                                      new MetricCriteria("LOC", OperationEnum.GREATER_THAN, 750),
                                      new MetricCriteria("[NOM] + [NOF]", OperationEnum.GREATER_THAN, 20)
                                      ),
                                  SmellType.GOD_CLASS);
            Rule rule3 = new Rule("10.1109/MSR.2007.21",
                                  new OrCriteria(
                                            new MetricCriteria("NMD", OperationEnum.GREATER_THAN, 15),
                                            new MetricCriteria("NAD", OperationEnum.GREATER_THAN, 15)),
                                  SmellType.GOD_CLASS);
            _rules = new List<Rule>();
            _rules.Add(rule1);
            _rules.Add(rule2);
            _rules.Add(rule3);
        }

        public PartialSmellDetectionReport FindIssues(List<CaDETClassDTO> caDetClassDtoList)
        {
            PartialSmellDetectionReport partialReport = new PartialSmellDetectionReport();

            foreach(CaDETClassDTO caDETClassDTO in caDetClassDtoList)
            {
                List<Issue> issues = ApplyRule(caDETClassDTO);
                foreach (Issue issue in issues)
                {
                    if (issue != null)
                    {
                        partialReport.AddIssue(issue.CodeItemId, issue);
                    }
                }
            }
            return partialReport;
        }

        public PartialSmellDetectionReport FindIssues(CaDETClassDTO caDetClassDto)
        {
            throw new NotImplementedException();
        }

        private List<Issue> ApplyRule(CaDETClassDTO c)
        {
            return _rules.Select(r => r.IsTriggered(c.FullName, c.CodeItemMetrics)).ToList();
        }
    }
}
