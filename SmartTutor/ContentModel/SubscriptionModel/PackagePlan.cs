using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.SubscriptionModel
{
    public class PackagePlan : SubscriptionPlan
    {
        public List<SubscriptionPackage> Teachers { get; set; }
    }
}