using SmartTutor.ContentModel.LearningObjects;

namespace SmartTutor.ProgressModel.Submissions
{
    public class AnswerEvaluation
    {
        public QuestionAnswer FullAnswer { get; set; }
        public bool SubmissionWasCorrect { get; set; }
    }
}
