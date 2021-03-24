using System;
using System.Collections.Generic;
using SmellDetector.Controllers;
using SmellDetector.SmellModel;

namespace SmellDetector.Detectors.RuleEngines
{
    public class Rule
    {
        private readonly String _DOI;
        private readonly Criteria _criteria;
        private readonly SmellType _smellType;
        public Rule(String DOI, Criteria criteria, SmellType smellType)
        {
            _DOI = DOI;
            _criteria = criteria;
            _smellType = smellType;
        }

        public Issue IsTriggered(String codeSnippetId, Dictionary<String, double> metrics)
        {
            if (_criteria.MeetCriteria(metrics))
            {
                Issue newIssue = new Issue();
                newIssue.IssueType = _smellType;
                newIssue.CodeItemId = codeSnippetId;
                return newIssue;
            }
            return null;
        }


    }
}
