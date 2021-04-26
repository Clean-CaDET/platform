using SmartTutor.ProgressModel.Feedback;
using SmartTutor.ProgressModel.Feedback.Repository;
using System;

namespace SmartTutor.ProgressModel
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
            feedback.TimeStamp = DateTime.Now;
            _feedbackRepository.SaveOrUpdate(feedback);
        }
    }
}