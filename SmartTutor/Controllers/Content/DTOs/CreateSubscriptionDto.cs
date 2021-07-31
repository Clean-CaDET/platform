namespace SmartTutor.Controllers.Content.DTOs
{
    public class CreateSubscriptionDto
    {
        public int TeacherId { get; set; }
        public int IndividualPlanId { get; set; }

        public CreateSubscriptionDto()
        {
        }
    }
}