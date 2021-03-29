using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.MetricRules
{
    public class MetricRangeRule
    {
        public CaDETMetric MetricName { get; internal set; }
        public double FromValue { get; internal set; }
        public double ToValue { get; internal set; }

        internal bool MetricMeetsRequirements(Dictionary<CaDETMetric, double> metrics)
        {
            var metricValue = metrics[MetricName];
            return (FromValue <= metricValue && metricValue <= ToValue);
        }
    }
}
