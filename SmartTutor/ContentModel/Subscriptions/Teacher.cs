using System.Collections.Generic;
using SmartTutor.ProgressModel.Submissions;

namespace SmartTutor.ContentModel.Subscriptions
{
    public class Teacher
    {
        public int Id { get; private set; }
        public List<Subscription> Subscriptions { get; private set; }
        
        public Subscription GetActiveSubscription()
        {
            return Subscriptions.Find(subscription => subscription.IsValid());
        }

        public void AddSubscription(Subscription subscription)
        {
            Subscriptions.Add(subscription);
        }
    }
}