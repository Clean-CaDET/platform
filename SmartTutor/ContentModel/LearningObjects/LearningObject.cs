using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.LearningObjects
{
    public class LearningObject
    {
        [Key] public int Id { get; set; }
        public int LearningObjectSummaryId { get; set; }
    }
}