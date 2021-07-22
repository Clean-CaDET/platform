using System;

namespace SmartTutor.ContentModel.Subscriptions
{
    public class Subscription
    {
        public int Id { get; private set; }
        public int TeacherId { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        
        public IndividualPlanUsage PlanUsage { get;  set; }

        public Subscription(int teacherId, DateTime start, DateTime end,IndividualPlanUsage  individualPlanUsage)
        {
            TeacherId = teacherId;
            Start = start;
            End = end;
            PlanUsage = individualPlanUsage;
        }

        public bool IsValid()
        {
            return DateTime.Now >= Start && DateTime.Now < End;
        }
    }
}