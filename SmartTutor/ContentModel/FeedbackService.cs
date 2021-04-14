using SmartTutor.ContentModel.FeedbackModel;
using SmartTutor.ContentModel.FeedbackModel.Repository;

namespace SmartTutor.ContentModel
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public void SubmitFeedback(LearningObjectFeedback feedback)
        {
            _feedbackRepository.SaveFeedback(feedback);
        }
    }
}