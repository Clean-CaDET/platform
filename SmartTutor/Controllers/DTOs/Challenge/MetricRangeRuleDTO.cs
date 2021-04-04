namespace SmartTutor.Controllers.DTOs.Challenge
{
    public class MetricRangeRuleDTO
    {
        public int Id { get; set; }
        public string MetricName { get; set; }
        public double FromValue { get; set; }
        public double ToValue { get; set; }
        public ChallengeHintDTO Hint { get; set; }

    }
}
