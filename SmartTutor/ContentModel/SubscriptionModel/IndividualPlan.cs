using System.Collections.Generic;

namespace SmartTutor.ContentModel.SubscriptionModel
{
    public class IndividualPlan : SubscriptionPlan
    {
        public List<Subscription> Subscriptions { get; set; }
    }
}