using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.LearningObjects
{
    public class QuestionAnswer
    {
        [Key] public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public string Feedback { get; set; }
    }
}