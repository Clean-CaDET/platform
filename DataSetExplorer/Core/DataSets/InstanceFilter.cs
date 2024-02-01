using System;
using System.Collections.Generic;
using System.Linq;
using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets.Model;

namespace DataSetExplorer.Core.DataSets
{
    internal class InstanceFilter
    {
        public List<SmellFilter> SmellFilters { get; }

        public InstanceFilter(List<SmellFilter> smellFilters)
        {
            SmellFilters = smellFilters;
        }

        public List<Instance> FilterInstances(CodeSmell codeSmell, List<Instance> instances, int numOfInstances)
        {
            var filteredInstances = new List<Instance>();
            var metricsThresholds = SmellFilters.Find(f => f.CodeSmell.Equals(codeSmell)).MetricsThresholds;

            foreach (var i in instances)
            {
                if (!ValidSnippetTypeForSmell(i, codeSmell)) continue;
                if (InstancePassesMetricThresholds(i, metricsThresholds)) filteredInstances.Add(i);
            }

            if (numOfInstances != 0) filteredInstances = filteredInstances.Take(numOfInstances).ToList();
            return filteredInstances;
        }

        private static bool ValidSnippetTypeForSmell(Instance instance, CodeSmell codeSmell)
        {
            return codeSmell.SnippetType.Equals(instance.Type);
        }

        private static bool InstancePassesMetricThresholds(Instance instance, List<MetricThresholds> metricsThreholds)
        {
            return metricsThreholds.TrueForAll(thresholds => IsInThresholdRange(instance, thresholds));
        }

        private static bool IsInThresholdRange(Instance instance, MetricThresholds thresholds)
        {
            var metric = (CaDETMetric)Enum.Parse(typeof(CaDETMetric), thresholds.Metric);
            var metricValue = instance.MetricFeatures.GetValueOrDefault(metric);

            double minValue = thresholds.MinValue.Equals("") ? Double.MinValue : Double.Parse(thresholds.MinValue);
            double maxValue = thresholds.MaxValue.Equals("") ? Double.MaxValue : Double.Parse(thresholds.MaxValue);
            return metricValue >= minValue && metricValue <= maxValue;
        }
    }
}
