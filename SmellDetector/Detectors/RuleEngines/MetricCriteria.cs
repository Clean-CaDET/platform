using System;
using System.Collections.Generic;
using SmellDetector.Controllers;

namespace SmellDetector.Detectors.RuleEngines
{
    public class MetricCriteria : Criteria
    {
        //This should be a single metric, so that we can compute the rule.
        private readonly String _metric;
        private readonly OperationEnum _operation;
        private readonly double _threshold;
        public MetricCriteria(String metric, OperationEnum operation, double threshold)
        {
            _metric = metric;
            _operation = operation;
            _threshold = threshold;
        }

        //This method should calculate the rule based on metric, operator and threshold and return true if the rule applies, otherwise false.
        public bool MeetCriteria(Dictionary<String, double> metrics)
        {
            if (!metrics.ContainsKey(_metric))
            {
                return false;
            }

            switch (_operation)
            {
                case OperationEnum.EQUALS:
                    return metrics[_metric] == _threshold;
                case OperationEnum.GREATER_THAN:
                    return metrics[_metric] > _threshold;
                case OperationEnum.LESS_THAN:
                    return metrics[_metric] < _threshold;
                case OperationEnum.GREATER_OR_EQUALS:
                    return metrics[_metric] >= _threshold;
                case OperationEnum.LESS_OR_EQUALS:
                    return metrics[_metric] <= _threshold;

            }
            return false;
        }
    }
}
