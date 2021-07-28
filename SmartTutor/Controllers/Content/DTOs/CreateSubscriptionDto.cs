namespace SmartTutor.Controllers.Content.DTOs
{
    public class CreateSubscriptionDto
    {
        public SubscriptionDto Subscription { get; set; }
        public int IndividualPlanId { get; set; }

        public CreateSubscriptionDto()
        {
        }
    }
}