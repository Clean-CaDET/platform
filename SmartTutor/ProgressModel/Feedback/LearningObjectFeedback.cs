using System;

namespace SmartTutor.ProgressModel.Feedback
{
    public class LearningObjectFeedback
    {
        public int Id { get; private set; }
        public int Rating { get; private set; }
        public int LearnerId { get; private set; }
        public int LearningObjectId { get; private set; }
        public DateTime TimeStamp { get; private set; } = DateTime.Now;

        public void UpdateRating(int feedbackRating)
        {
            Rating = feedbackRating;
            TimeStamp = DateTime.Now;
        }
    }
}