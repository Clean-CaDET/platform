using SmartTutor.ContentModel.LearningObjects;

namespace SmartTutor.ContentModel.ProgressModel
{
    public class AnswerEvaluation
    {
        //TODO: Will need to expand to track what the submitted solution was
        public QuestionAnswer FullAnswer { get; set; }
        public bool SubmissionWasCorrect { get; set; }
    }
}
