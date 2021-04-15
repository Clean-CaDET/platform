using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.ProgressModel
{
    //TODO: Submissions and evaluations should probably go to the progress model.
    public class ArrangeTaskSubmission
    {
        [Key] public int Id { get; set; }
        public int ArrangeTaskId { get; set; }
        public int TraineeId { get; set; }
        public bool IsCorrect { get; set; }
        public List<ArrangeTaskContainerSubmission> Containers { get; set; }
    }
}