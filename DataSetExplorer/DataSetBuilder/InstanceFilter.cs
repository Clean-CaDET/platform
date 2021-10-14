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
            if (CheckIfInRange(thresholds))
            {
                return IsMetricValueInRange(metricValue, thresholds);
            }
            else if (CheckIfGreaterThanMinValue(thresholds))
            {
                return IsMetricValueGreaterThan(metricValue, thresholds.MinValue);
            }
            else if (CheckIfLessThanMaxValue(thresholds))
            {
                return IsMetricValueLessThan(metricValue, thresholds.MaxValue);
            }
            return true;
        }

        private static bool CheckIfInRange(MetricThresholds thresholds)
        {
            return !thresholds.MinValue.Equals("") && !thresholds.MaxValue.Equals("");
        }

        private static bool IsMetricValueInRange(double metricValue, MetricThresholds thresholds)
        {
            if (metricValue > Double.Parse(thresholds.MinValue) && metricValue < Double.Parse(thresholds.MaxValue)) return true;
            else return false;
        }

        private static bool CheckIfGreaterThanMinValue(MetricThresholds thresholds)
        {
            return !thresholds.MinValue.Equals("");
        }

        private static bool IsMetricValueGreaterThan(double metricValue, string minValue)
        {
            if (metricValue > Double.Parse(minValue)) return true;
            else return false;
        }

        private static bool CheckIfLessThanMaxValue(MetricThresholds thresholds)
        {
            return !thresholds.MaxValue.Equals("");
        }

        private static bool IsMetricValueLessThan(double metricValue, string maxValue)
        {
            if (metricValue < Double.Parse(maxValue)) return true;
            else return false;
        }
    }
}
