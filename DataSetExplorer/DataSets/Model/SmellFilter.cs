using System.Collections.Generic;

namespace DataSetExplorer.DataSets.Model
{
    public class SmellFilter
    {
        public CodeSmell CodeSmell { get; private set; }
        public List<MetricThresholds> MetricsThresholds { get; private set; }

        public SmellFilter() { }

        public SmellFilter(CodeSmell codeSmell, List<MetricThresholds> metricsThresholds)
        {
            CodeSmell = codeSmell;
            MetricsThresholds = metricsThresholds;
        }

        public override int GetHashCode()
        {
            return CodeSmell.GetHashCode();
        }

        public override bool Equals(object other)
        {
            return other is CodeSmell smell && CodeSmell.Equals(smell);
        }
    }
}