using DataSetExplorer.DataSetBuilder.Model;
using System.Collections.Generic;
using CodeModel.CaDETModel.CodeItems;
using System;

namespace DataSetExplorer.DataSetBuilder
{
    internal class InstanceFilter
    {
        public List<MetricThresholds> MetricThresholds { get; }
        
        public InstanceFilter(List<MetricThresholds> metricThresholds)
        {
            MetricThresholds = metricThresholds;
        }

        public List<Instance> FilterInstances(CodeSmell codeSmell, List<Instance> instances)
        {
            var filteredInstances = new List<Instance>();
            var metricsForSmell = MetricThresholds.FindAll(m => m.CodeSmell.Equals(codeSmell.Name));

            foreach (var i in instances)
            {
                if (!ValidSnippetTypeForSmell(i, codeSmell)) continue;
                var fulfilledConditions = new List<bool>();
                foreach (var thresholds in metricsForSmell)
                {
                    fulfilledConditions.Add(InstanceFulfillesConditions(i, thresholds));
                }
                if (!fulfilledConditions.Contains(false)) filteredInstances.Add(i);
            }
            return filteredInstances;
        }

        private static bool ValidSnippetTypeForSmell(Instance instance, CodeSmell codeSmell)
        {
            return codeSmell.RelevantSnippetTypes()[0].ToString().Equals(instance.Type.ToString());
        }

        private static bool InstanceFulfillesConditions(Instance instance, MetricThresholds thresholds)
        {
            var metric = (CaDETMetric)Enum.Parse(typeof(CaDETMetric), thresholds.Metric);
            var metricValue = instance.MetricFeatures.GetValueOrDefault(metric);

            double minValue = thresholds.MinValue.Equals("") ? Double.MinValue : Double.Parse(thresholds.MinValue);
            double maxValue = thresholds.MaxValue.Equals("") ? Double.MaxValue : Double.Parse(thresholds.MaxValue);
            return metricValue >= minValue && metricValue <= maxValue;
        }
    }
}
