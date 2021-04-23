namespace SmartTutor.ProgressModel.Feedback.Repository
{
    public interface IFeedbackRepository
    {
        void SaveFeedback(LearningObjectFeedback feedback);
        void UpdateFeedback(LearningObjectFeedback feedback);
        LearningObjectFeedback GetFeedback(int learningObjectId, int traineeId);
    }
}