using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ProgressModel.Submissions
{
    public class ArrangeTaskSubmission
    {
        [Key] public int Id { get; set; }
        public int ArrangeTaskId { get; set; }
        public int TraineeId { get; set; }
        public bool IsCorrect { get; set; }
        public List<ArrangeTaskContainerSubmission> Containers { get; set; }
    }
}