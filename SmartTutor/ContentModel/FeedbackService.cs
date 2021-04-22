using SmartTutor.ContentModel.Feedback;
using System;
using SmartTutor.ContentModel.Feedback.Repository;

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
            var loadedFeedback = _feedbackRepository.GetFeedback(feedback.LearningObjectId, feedback.TraineeId);
            if (loadedFeedback == null) _feedbackRepository.SaveFeedback(feedback);
            else _feedbackRepository.UpdateFeedback(feedback);
        }
    }
}