using System;
using System.Collections.Generic;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;

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

        public PartialSmellDetectionReport FindIssues(List<CaDETClassDTO> caDetClassDtoList)
        {

            PartialSmellDetectionReport partialReport = new PartialSmellDetectionReport();

            foreach (CaDETClassDTO caDETClassDTO in caDetClassDtoList)
            {
                List<Issue> issues = ApplyRule(caDETClassDTO);
                foreach (Issue issue in issues)
                {
                    if (issue != null)
                    {
                        partialReport.AddIssue(issue.CodeSnippetId, issue);
                    }
                }
            }
            return partialReport;
            _rule.Validate()
            //if(cloc>300) call method rule engine -> partial issue if smelltype == longmethod ++brojac
            //if brojac == 5
            // return true partial...
            throw new NotImplementedException();
        }

        private Issue ApplyRule(CaDETClassDTO c)
        {
            return _rule.Validate(c.FullName, c.CodeSnippetMetrics);
        }
    }
}
