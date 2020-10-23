using System;
using SmellDetector.DTO;

public class LargeClass : SmellRule
{
    public override bool isBadSmell(MetricsDTO metrics)
    {
        throw new NotImplementedException();
    }
}