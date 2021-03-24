using System;
using System.Collections.Generic;

namespace SmellDetector.Detectors.RuleEngines
{
    public class AndCriteria : Criteria
    {
        private readonly Criteria _firstCriteria;
        private readonly Criteria _secondCriteria;
        public AndCriteria(Criteria firstCriteria, Criteria secondCriteria)
        {
            _firstCriteria = firstCriteria;
            _secondCriteria = secondCriteria;
        }

        public bool MeetCriteria(Dictionary<String, double> metrics)
        {
            return _firstCriteria.MeetCriteria(metrics) && _secondCriteria.MeetCriteria(metrics);
        }
    }
}
