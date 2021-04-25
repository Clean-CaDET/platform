using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.SubscriptionModel
{
    public class SubscriptionPackage
    {
        [Key] public int Id { get; set; }
        public int OrganizationId { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}