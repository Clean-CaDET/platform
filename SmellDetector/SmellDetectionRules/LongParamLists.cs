using SmellDetector.Controllers;
using System;

namespace SmellDetector.SmellDetectionRules
{
    public class LongParamLists : SmellRule
    {
        public bool isBadSmell(MetricsDTO metrics)
        {
            if (metrics.NOP > 5) return true;
            return false;
        }
    }
}
