using System;

namespace SmartTutor.ContentModel.Subscriptions
{
    public class Subscription
    {
        public int Id { get; private set; }
        public int TeacherId { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        
        public IndividualPlanUsage PlanUsage { get; private set; }

        public bool IsValid()
        {
            return DateTime.Now >= Start && DateTime.Now < End;
        }
    }
}