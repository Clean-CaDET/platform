using System;
using System.Collections.Generic;
using SmellDetector.Controllers;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;

namespace SmellDetector.Detectors.RuleEngines
{
    internal class GodClassMetricRuleEngine: IDetector
    {
        private Dictionary<Guid, Criteria> criterias;
        public GodClassMetricRuleEngine(Dictionary<Guid, Criteria> criterias)
        {
            this.criterias = criterias;
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

        private bool IsBadSmell(MetricsDTO value)
        {
            bool isBadSmell = false;
            foreach(Criteria criteria in criterias.Values)
            {
                isBadSmell = criteria.meetCriteria();
            }
            return isBadSmell;
        }
    }
}
