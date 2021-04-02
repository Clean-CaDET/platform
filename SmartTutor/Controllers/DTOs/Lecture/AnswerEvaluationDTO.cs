namespace SmartTutor.Controllers.DTOs.Lecture
{
    public class AnswerEvaluationDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Feedback { get; set; }
        public bool SubmissionWasCorrect { get; set; }
    }
}