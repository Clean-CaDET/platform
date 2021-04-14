using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.ProgressModel
{
    public class ChallengeSubmission
    {
        [Key] public int Id { get; set; }
        public string[] SubmittedCode { get; set; }
        public string TraineeId { get; set; }
        public int ChallengeId { get; set; }
        public bool IsCorrect { get; set; }
    }
}