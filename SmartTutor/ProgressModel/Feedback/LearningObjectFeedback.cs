using System;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ProgressModel.Feedback
{
    public class LearningObjectFeedback
    {
        [Key] public int Id { get; set; }
        public int Rating { get; set; }
        public int LearnerId { get; set; }
        public int LearningObjectId { get; set; }
        public DateTime TimeStamp { get; set; }

        public void UpdateRating(int feedbackRating)
        {
            Rating = feedbackRating;
            TimeStamp = DateTime.Now;
        }
    }
}