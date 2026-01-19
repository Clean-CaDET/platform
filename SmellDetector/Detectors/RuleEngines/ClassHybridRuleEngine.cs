using CodeModel.CaDETModel.CodeItems;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;
using System.Collections.Generic;
using System.Linq;

namespace SmellDetector.Detectors.RuleEngines
{
    public class ClassHybridRuleEngine: IDetector
    {
        private readonly Rule _rule;
        private readonly int _relatedSmellsCount;
        private readonly MethodMetricRuleEngine _methodMetricRuleEngine;

        public ClassHybridRuleEngine()
        {
            _rule = new Rule("",
                            new MetricCriteria(CaDETMetric.CLOC, OperationEnum.GREATER_THAN, 300),
                            SmellType.GOD_CLASS);
            _relatedSmellsCount = 5;
            _methodMetricRuleEngine = new MethodMetricRuleEngine();
        }

        public PartialSmellDetectionReport FindIssues(List<CaDETClass> classes)
        {
            var partialReport = new PartialSmellDetectionReport();

            foreach (var caDETClass in classes)
            {
                var metricRuleIssue = ApplyRule(caDETClass);
                if (metricRuleIssue == null) continue;

                var methodIssuesReport = _methodMetricRuleEngine.FindIssues(new List<CaDETClass> { caDETClass });
                if(ClassHasEnoughLongMethods(methodIssuesReport))
                {
                    partialReport.AddIssue(caDETClass.FullName,
                        new Issue(SmellType.GOD_CLASS, caDETClass.FullName));
                }
            }
            return partialReport;
        }

        private bool ClassHasEnoughLongMethods(PartialSmellDetectionReport report)
        {
            int longMethodCounter = 0;
            foreach (List<Issue> issues in report.CodeSnippetIssues.Values)
            {
                longMethodCounter += issues.Count(issue => issue.IssueType == SmellType.LONG_METHOD);
            }

            return longMethodCounter >= _relatedSmellsCount;
        }

        private Issue ApplyRule(CaDETClass c)
        {
            return _rule.Validate(c.FullName, c.Metrics);
        }
    }
}
