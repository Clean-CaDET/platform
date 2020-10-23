using System;
using SmellDetector.DTO;

public class LongMethod : SmellRule
{
    public override bool isBadSmell(MetricsDTO metrics)
    {
        if (metrics.LOC > 50) return true;
        if (metrics.CYCLO > 5) return true;
        if (metrics.NOP > 4 || metrics.NOLV > 4) return true;

        return false;

    }
}