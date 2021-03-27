using RepositoryCompiler.CodeModel.CaDETModel.Metrics;
using System;

namespace SmartTutor.ContentModel.LearningObjects.MetricRules
{
    public class MetricRangeRule
    {
        public string MetricName { get; internal set; }
        public double FromValue { get; internal set; }
        public double ToValue { get; internal set; }

        // TODO: CaDETClassMetrics soon to be a Dictionary<string, double>, update
        public bool ClassMetricMeetsRequirements(CaDETClassMetrics metrics) => MetricMeetsRequirements(metrics);

        public bool MethodMetricMeetsRequirements(CaDETMemberMetrics metrics) => MetricMeetsRequirements(metrics);

        private bool MetricMeetsRequirements(object metrics)
        {
            object metricObjectValue = metrics.GetType().GetProperty(MetricName).GetValue(metrics, null);
            double metricValue = Convert.ToDouble(metricObjectValue);
            return (FromValue <= metricValue && metricValue <= ToValue);
        }
    }
}
