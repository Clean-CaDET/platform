using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.LectureModel
{
    public class LearningObjectSummary
    {
        [Key] public int Id { get; set; }
        public string Description { get; set; }
    }
}