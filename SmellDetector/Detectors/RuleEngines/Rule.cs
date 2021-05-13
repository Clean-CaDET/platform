using CodeModel.CaDETModel.CodeItems;
using SmellDetector.SmellModel;
using System.Collections.Generic;

namespace SmellDetector.Detectors.RuleEngines
{
    public class Rule
    {
        private readonly string _DOI;
        private readonly Criteria _criteria;
        private readonly SmellType _smellType;
        public Rule(string DOI, Criteria criteria, SmellType smellType)
        {
            _DOI = DOI;
            _criteria = criteria;
            _smellType = smellType;
        }

        public Issue Validate(string codeSnippetId, Dictionary<CaDETMetric, double> metrics)
        {
            if (_criteria.MeetCriteria(metrics))
            {
                return new Issue {IssueType = _smellType, CodeSnippetId = codeSnippetId};
            }
            return null;
        }


    }
}
