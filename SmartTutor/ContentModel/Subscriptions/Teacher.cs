using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SmartTutor.ContentModel.Lectures;

namespace SmartTutor.ContentModel.Subscriptions

{
    public class Teacher
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public List<Subscription> Subscriptions { get; private set; }
        public List<Course> Courses { get; private set; }
        
        public Subscription GetActiveSubscription()
        {
            return Subscriptions.Find(subscription => subscription.IsValid());
        }

        public void AddSubscription(Subscription subscription)
        {
            Subscriptions.Add(subscription);
        }
        
        public void AddCourse(Course course)
        {
            if (Courses == null) Courses = new List<Course>();
            Courses.Add(course);
        }
    }
}