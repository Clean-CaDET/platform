namespace SmartTutor.ContentModel.LearningObjects.Questions
{
    public class AnswerEvaluation
    {
        public QuestionAnswer FullAnswer { get; }
        public bool SubmissionWasCorrect { get; }

        public AnswerEvaluation(QuestionAnswer answer, bool isCorrect)
        {
            FullAnswer = answer;
            SubmissionWasCorrect = isCorrect;
        }
    }
}
