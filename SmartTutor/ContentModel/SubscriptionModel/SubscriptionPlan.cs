using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.SubscriptionModel
{
    public abstract class SubscriptionPlan
    {
        [Key] public int Id { get; set; }
        public Payment Payment { get; set; }
        public int NumOfCourses { get; set; }
        public int NumOfLectures { get; set; }
    }
}