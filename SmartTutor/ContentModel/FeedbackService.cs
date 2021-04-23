using SmartTutor.ProgressModel.Feedback;
using SmartTutor.ProgressModel.Feedback.Repository;
using System;

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
            feedback.TimeStamp = DateTime.Now;
            var loadedFeedback = _feedbackRepository.GetFeedback(feedback.LearningObjectId, feedback.LearnerId);
            if (loadedFeedback == null) _feedbackRepository.SaveFeedback(feedback);
            else _feedbackRepository.UpdateFeedback(feedback);
        }
    }
}