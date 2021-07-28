using System;

namespace SmartTutor.ContentModel.Subscriptions
{
    public abstract class SubscriptionPlan
    {
        public int Id { get; set; }
        public int NumberOfUsers { get; set; }
        public int NumberOfCourses { get; set; }
        public int NumberOfLectures { get; set; }
        public TimeSpan Duration { get; set; }
    }
}