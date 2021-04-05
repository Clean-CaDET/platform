using SmartTutor.ContentModel.LectureModel;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy
{
    public class ChallengeHint
    {
        [Key] public int Id { get; set; }
        public string Content { get; set; }
        public LearningObjectSummary LearningObjectSummary { get; set; }
    }
}
