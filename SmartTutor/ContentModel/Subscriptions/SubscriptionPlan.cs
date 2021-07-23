using System.ComponentModel.DataAnnotations.Schema;
using SmartTutor.ContentModel.Lectures;

namespace SmartTutor.ContentModel.Subscriptions
{
    public abstract class SubscriptionPlan
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public int NumberOfUsers { get; protected set; }
        public int NumberOfCourses { get; private set; }
        public int NumberOfLectures { get; private set; }
    }
}