using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.SubscriptionModel
{
    public class Subscription
    {
        [Key] public int Id { get; set; }
        public int TeacherId { get; set; }
        public int PlanId { get; set; }
    }
}