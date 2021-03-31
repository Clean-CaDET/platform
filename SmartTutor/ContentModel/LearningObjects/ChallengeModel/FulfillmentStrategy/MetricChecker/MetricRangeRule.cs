using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.MetricChecker
{
    public class MetricRangeRule
    {
        [Key] public int Id { get; set; }
        public string MetricName { get; internal set; }
        public double FromValue { get; internal set; }
        public double ToValue { get; internal set; }

        internal bool MetricMeetsRequirements(Dictionary<CaDETMetric, double> metrics)
        {
            var metricValue = metrics[(CaDETMetric)Enum.Parse(typeof(CaDETMetric), MetricName, true)];
            return (FromValue <= metricValue && metricValue <= ToValue);
        }
    }
}
