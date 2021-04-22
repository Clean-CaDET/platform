using System;
using System.Collections.Generic;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;

namespace SmellDetector.Detectors.RuleEngines
{
    public class OrCriteria : Criteria
    {
        private readonly Criteria _firstCriteria;
        private readonly Criteria _secondCriteria;
        public OrCriteria(Criteria firstCriteria, Criteria secondCriteria)
        {
            _firstCriteria = firstCriteria;
            _secondCriteria = secondCriteria;
        }

        public bool MeetCriteria(Dictionary<CaDETMetric, double> metrics)
        {
            return _firstCriteria.MeetCriteria(metrics) || _secondCriteria.MeetCriteria(metrics);
        }
    }
}
