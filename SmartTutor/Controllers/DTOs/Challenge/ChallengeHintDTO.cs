using SmartTutor.Controllers.DTOs.Lecture;

namespace SmartTutor.Controllers.DTOs.Challenge
{
    public class ChallengeHintDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public LearningObjectSummaryDTO LearningObjectSummary { get; set; }
    }
}
