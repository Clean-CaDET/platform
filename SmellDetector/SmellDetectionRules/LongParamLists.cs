using System;
using SmellDetector.DTO;

public class LongParamLists : SmellRule
{
    public override bool isBadSmell(MetricsDTO metrics)
    {
        if (metrics.NOP > 5) return true;
        return false;
    }
}