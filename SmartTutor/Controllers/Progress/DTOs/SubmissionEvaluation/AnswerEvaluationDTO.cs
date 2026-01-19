namespace SmartTutor.Controllers.Progress.DTOs.SubmissionEvaluation
{
    public class AnswerEvaluationDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Feedback { get; set; }
        public bool SubmissionWasCorrect { get; set; }
    }
}