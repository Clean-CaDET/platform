using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.MetricChecker
{
    public class MetricRangeRule
    {
        [Key] public int Id { get; set; }
        public string MetricName { get; set; }
        public double FromValue { get; set; }
        public double ToValue { get; set; }
        public ChallengeHint Hint { get; set; }

        internal ChallengeHint Evaluate(Dictionary<CaDETMetric, double> metrics)
        {
            var metricValue = metrics[(CaDETMetric)Enum.Parse(typeof(CaDETMetric), MetricName, true)];
            var isFulfilled = FromValue <= metricValue && metricValue <= ToValue;
            return isFulfilled ? null : Hint;
        }
    }
}
