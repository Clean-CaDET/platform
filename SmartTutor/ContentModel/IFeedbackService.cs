using SmartTutor.ContentModel.FeedbackModel;

namespace SmartTutor.ContentModel
{
    public interface IFeedbackService
    {
        void SubmitFeedback(LearningObjectFeedback feedback);
    }
}