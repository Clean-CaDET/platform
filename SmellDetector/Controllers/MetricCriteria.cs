using System;
namespace SmellDetector.Controllers
{
    public class MetricCriteria:Criteria
    {
        private MetricsDTO metric; //This should be a single metric, so that we can compute the rule.
        private OperationEnum operation;
        private double threshold;
        public MetricCriteria(MetricsDTO metric, OperationEnum operation, double threshold)
        {
            this.metric = metric;
            this.operation = operation;
            this.threshold = threshold;
        }

        //This method should calculate the rule based on metric, operator and threshold and return true if the rule applies, otherwise false.
        public bool meetCriteria()
        {
            switch (operation)
            {
                case OperationEnum.EQUALS: return true;
                case OperationEnum.GREATER_THAN: return true;
                case OperationEnum.LESS_THAN: return true;
                case OperationEnum.GREATER_OR_EQUALS: return true;
                case OperationEnum.LESS_OR_EQUALS: return true;

            }
            return false;
        }
    }
}
