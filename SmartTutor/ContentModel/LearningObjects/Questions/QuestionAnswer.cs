namespace SmartTutor.ContentModel.LearningObjects.Questions
{
    public class QuestionAnswer
    {
        public int Id { get; private set; }
        public int QuestionId { get; private set; }
        public string Text { get; private set; }
        public bool IsCorrect { get; private set; }
        public string Feedback { get; private set; }

        public QuestionAnswer(int id, int questionId, string text, bool isCorrect, string feedback)
        {
            Id = id;
            QuestionId = questionId;
            Text = text;
            IsCorrect = isCorrect;
            Feedback = feedback;
        }
    }
}