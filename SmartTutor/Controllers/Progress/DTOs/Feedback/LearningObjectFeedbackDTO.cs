using System.ComponentModel.DataAnnotations;

namespace SmartTutor.Controllers.Progress.DTOs.Feedback
{
    public class LearningObjectFeedbackDTO
    {
        public int Id { get; set; }
        [Range(1, 5)] public int Rating { get; set; }
        public int LearnerId { get; set; }
        public int LearningObjectId { get; set; }
    }
}