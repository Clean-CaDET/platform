using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.MetricChecker
{
    public class MetricRangeRule
    {
        [Key] public int Id { get; set; }
        public string MetricName { get; set; }
        public double FromValue { get; set; }
        public double ToValue { get; set; }
        public ChallengeHint Hint { get; set; }

        internal bool MetricMeetsRequirements(Dictionary<CaDETMetric, double> metrics)
        {
            var metricValue = metrics[(CaDETMetric)Enum.Parse(typeof(CaDETMetric), MetricName, true)];
            return (FromValue <= metricValue && metricValue <= ToValue);
        }
    }
}
