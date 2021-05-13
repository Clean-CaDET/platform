using CodeModel.CaDETModel.CodeItems;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;
using System.Collections.Generic;
using System.Linq;

namespace SmellDetector.Detectors.RuleEngines
{
    public class ClassHybridRuleEngine: IDetector
    {
        Rule _rule;
        int _relatedSmellsCount;

        public ClassHybridRuleEngine()
        {
            _rule = new Rule("",
                            new MetricCriteria(CodeModel.CaDETModel.CodeItems.CaDETMetric.CLOC, OperationEnum.GREATER_THAN, 300),
                            SmellType.GOD_CLASS);
            _relatedSmellsCount = 5;
        }

        public PartialSmellDetectionReport FindIssues(List<CaDETClass> caDetClassList)
        {
            PartialSmellDetectionReport partialReport = new();

            foreach (CaDETClass caDETClass in caDetClassList)
            {
                Issue metricRuleIssue = ApplyRule(caDETClass);
                if(metricRuleIssue != null)
                {
                    PartialSmellDetectionReport methodIssuesReport = CreateMethodsIssuesReport(caDETClass);
                    if(ClassHasEnoughLongMethods(methodIssuesReport))
                    {
                        partialReport.AddIssue(caDETClass.FullName,
                                                new() { IssueType = SmellType.GOD_CLASS, CodeSnippetId = caDETClass.FullName });
                    }
                }
            }
            return partialReport;
        }

        private PartialSmellDetectionReport CreateMethodsIssuesReport(CaDETClass caDETClass)
        {
            MethodMetricRuleEngine methodMetricRuleEngine = new();
            PartialSmellDetectionReport report = methodMetricRuleEngine.FindIssues(new List<CaDETClass> { caDETClass });
            return report;
        }

        private bool ClassHasEnoughLongMethods(PartialSmellDetectionReport report)
        {
            int longMethodCounter = 0;
            foreach (List<Issue> issues in report.CodeItemIssues.Values)
            {
                longMethodCounter += issues.Select(issue => issue.IssueType == SmellType.LONG_METHOD).Count();
                if (longMethodCounter >= _relatedSmellsCount)
                {
                    return true;
                }
            }

            return false;
        }

        private Issue ApplyRule(CaDETClass c)
        {
            return _rule.Validate(c.FullName, c.Metrics);
        }
    }
}
