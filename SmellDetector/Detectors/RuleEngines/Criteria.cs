using System;
using System.Collections.Generic;
using CodeModel.CaDETModel.CodeItems;

namespace SmellDetector.Detectors.RuleEngines
{
    public interface Criteria
    {
        public bool MeetCriteria(Dictionary<CaDETMetric, double> metrics);
    }
}
