using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.ProgressModel
{
    public class Trainee
    {
        [Key] public int Id { get; set; }
        public List<NodeProgress> Progress { get; set; }
    }
}