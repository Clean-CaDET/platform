using System.ComponentModel.DataAnnotations;

namespace SmartTutor.Controllers.DTOs.Feedback
{
    public class LearningObjectFeedbackDTO
    {
        public int Id { get; set; }
        [Range(1, 5)] public int Rating { get; set; }
        public int TraineeId { get; set; }
        public int LearningObjectId { get; set; }
    }
}