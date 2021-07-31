using System;

namespace SmartTutor.Controllers.Content.DTOs
{
    public class SubscriptionDto
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int IndividualPlanUsageId { get; set; }
        public TimeSpan Duration { get; set; }

        public SubscriptionDto()
        {
        }

        public SubscriptionDto(int id, int teacherId, int individualPlanUsageId, TimeSpan duration)
        {
            Id = id;
            TeacherId = teacherId;
            IndividualPlanUsageId = individualPlanUsageId;
            Duration = duration;
        }
    }
}