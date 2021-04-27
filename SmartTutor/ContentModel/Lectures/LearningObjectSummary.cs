using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.Lectures
{
    public class LearningObjectSummary
    {
        [Key] public int Id { get; set; }
        public string Description { get; set; }
    }
}