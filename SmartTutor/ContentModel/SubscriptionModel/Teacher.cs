using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.SubscriptionModel
{
    public class Teacher
    {
        [Key] public int Id { get; set; }
        public int CourseId { get; set; }
        public List<Subscription> Subscriptions { get; set; }
    }
}