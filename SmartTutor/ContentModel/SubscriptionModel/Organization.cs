using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.SubscriptionModel
{
    public class Organization
    {
        [Key] public int Id { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<SubscriptionPackage> SubscriptionPackages { get; set; }
        public List<Course> Courses { get; set; }
    }
}