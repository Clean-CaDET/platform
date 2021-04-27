using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ProgressModel.Submissions
{
    public class ArrangeTaskContainerSubmission
    {
        [Key] public int Id { get; set; }
        public int ContainerId { get; set; }
        public List<int> ElementIds { get; set; }
    }
}