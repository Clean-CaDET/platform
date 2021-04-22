using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ProgressModel.Submissions
{
    public class QuestionSubmission
    {
        [Key] public int Id { get; set; }
        public List<int> SubmittedAnswerIds { get; set; }
        public int QuestionId { get; set; }
        public int TraineeId { get; set; }
        public bool IsCorrect { get; set; }
    }
}
