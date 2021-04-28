namespace SmartTutor.ContentModel.LearningObjects.ArrangeTasks
{
    public class ArrangeTaskContainerEvaluation
    {
        public ArrangeTaskContainer FullAnswer { get; private set; }
        public bool SubmissionWasCorrect { get; private set; }

        public ArrangeTaskContainerEvaluation(ArrangeTaskContainer fullAnswer, bool submissionWasCorrect)
        {
            FullAnswer = fullAnswer;
            SubmissionWasCorrect = submissionWasCorrect;
        }
    }
}
