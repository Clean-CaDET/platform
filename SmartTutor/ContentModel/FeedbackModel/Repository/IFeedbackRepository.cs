namespace SmartTutor.ContentModel.FeedbackModel.Repository
{
    public interface IFeedbackRepository
    {
        void SaveFeedback(LearningObjectFeedback feedback);
    }
}