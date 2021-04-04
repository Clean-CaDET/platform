using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.Challenge
{
    public class BasicMetricsCheckerDTO
    {
        public List<MetricRangeRuleDTO> ClassMetricRules { get; set; }
        public List<MetricRangeRuleDTO> MethodMetricRules { get; set; }

    }
}
