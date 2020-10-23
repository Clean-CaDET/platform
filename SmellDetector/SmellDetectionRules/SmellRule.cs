using System;
using SmellDetector.DTO;

public abstract class SmellRule
{
   public abstract bool isBadSmell(MetricsDTO metrics);

}