using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTutor.ContentModel.FeedbackModel
{
    [Table("LearningObjectFeedbacks")]
    public class LearningObjectFeedback
    {
        [Key] public int Id { get; set; }
        public int Rating { get; set; }
        public int TraineeId { get; set; }
        public int LearningObjectId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}