namespace SmartTutor.ContentModel.DTOs
{
    public class SubscriptionDto
    {
        public int TeacherId {get; set; }
        public int NumberOfDays {get; set; }
        public int IndividualPlanId {get; set; }

        public SubscriptionDto()
        {
        }

        public SubscriptionDto(int teacherId, int numberOfDays, int individualPlanId)
        {
            TeacherId = teacherId;
            NumberOfDays = numberOfDays;
            IndividualPlanId = individualPlanId;
        }
    }
}