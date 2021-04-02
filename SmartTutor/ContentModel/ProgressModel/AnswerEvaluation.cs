using SmartTutor.ContentModel.LearningObjects;

namespace SmartTutor.ContentModel.ProgressModel
{
    public class AnswerEvaluation
    {
        //TODO: Tied to Question LO, represents the submitted answer and its correctness
        public QuestionAnswer FullAnswer { get; set; }
        public bool SubmissionWasCorrect { get; set; }
    }
}
