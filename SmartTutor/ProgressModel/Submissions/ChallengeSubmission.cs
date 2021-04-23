using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ProgressModel.Submissions
{
    public class ChallengeSubmission
    {
        [Key] public int Id { get; set; }
        public string[] SourceCode { get; set; }
        public string LearnerId { get; set; }
        public int ChallengeId { get; set; }
        public bool IsCorrect { get; set; }
    }
}