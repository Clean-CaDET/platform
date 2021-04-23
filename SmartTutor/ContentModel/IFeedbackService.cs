using SmartTutor.ProgressModel.Feedback;

namespace SmartTutor.ContentModel
{
    public interface IFeedbackService
    {
        void SubmitFeedback(LearningObjectFeedback feedback);
    }
}