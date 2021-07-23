using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTutor.ContentModel.Subscriptions
{
    public class Subscription
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public int TeacherId { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        
        public int PlanUsageId { get;  set; }

        public Subscription(int teacherId, DateTime start, DateTime end,int  individualPlanUsage)
        {
            TeacherId = teacherId;
            Start = start;
            End = end;
            PlanUsageId = individualPlanUsage;
        }

        public Subscription(int id, int teacherId, DateTime start, DateTime end, int planUsageId)
        {
            Id = id;
            TeacherId = teacherId;
            Start = start;
            End = end;
            PlanUsageId = planUsageId;
        }

        public bool IsValid()
        {
            return DateTime.Now >= Start && DateTime.Now < End;
        }
    }
}