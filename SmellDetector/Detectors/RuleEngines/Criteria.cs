using System;
using System.Collections.Generic;

namespace SmellDetector.Detectors.RuleEngines
{
    public interface Criteria
    {
        public bool MeetCriteria(Dictionary<String, double> metrics);
    }
}
