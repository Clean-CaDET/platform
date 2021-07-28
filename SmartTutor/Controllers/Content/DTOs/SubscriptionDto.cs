using System;

namespace SmartTutor.Controllers.Content.DTOs
{
    public class SubscriptionDto
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int IndividualPlanUsageId { get; set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public SubscriptionDto()
        {
        }

        public SubscriptionDto(int id, int teacherId, int individualPlanUsageId, DateTime start, DateTime end)
        {
            Id = id;
            TeacherId = teacherId;
            IndividualPlanUsageId = individualPlanUsageId;
            Start = start;
            End = end;
        }
    }
}