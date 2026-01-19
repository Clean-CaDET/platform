namespace SmartTutor.ProgressModel.Feedback.Repository
{
    public interface IFeedbackRepository
    {
        void SaveOrUpdate(LearningObjectFeedback feedback);
        LearningObjectFeedback Get(int learningObjectId, int learnerId);
    }
}