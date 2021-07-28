using System;

namespace SmartTutor.ContentModel.Subscriptions
{
    public class Subscription
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public int IndividualPlanUsageId { get; set; }

        public Subscription(int teacherId, DateTime start, DateTime end, int individualPlanUsageId) : this(0, teacherId,
            start, end, individualPlanUsageId)
        {
        }

        public Subscription(int id, int teacherId, DateTime start, DateTime end, int individualPlanUsageId)
        {
            Id = id;
            TeacherId = teacherId;
            Start = start;
            End = end;
            IndividualPlanUsageId = individualPlanUsageId;
        }

        public bool IsValid()
        {
            return DateTime.Now >= Start && DateTime.Now < End;
        }
    }
}