using SmartTutor.ContentModel.LearningObjects;

namespace SmartTutor.ContentModel.ProgressModel
{
    public class ArrangeTaskContainerEvaluation
    {
        //TODO: Will need to expand to track what the submitted solution was
        public ArrangeTaskContainer FullAnswer { get; set; }
        public bool SubmissionWasCorrect { get; set; }
    }
}
