using CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
namespace SmellDetector.Detectors.RuleEngines
{
    public interface Criteria
    {
        public bool MeetCriteria(Dictionary<CaDETMetric, double> metrics);
    }
}
