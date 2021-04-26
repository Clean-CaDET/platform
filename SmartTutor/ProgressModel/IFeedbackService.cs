using SmartTutor.ProgressModel.Feedback;

namespace SmartTutor.ProgressModel
{
    public interface IFeedbackService
    {
        void SubmitFeedback(LearningObjectFeedback feedback);
    }
}