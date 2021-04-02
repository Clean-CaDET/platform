using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.LearningObjects
{
    public class ArrangeTaskElement
    {
        [Key] public int Id { get; set; }
        public int ArrangeTaskContainerId { get; set; }
        public string Text { get; set; }
    }
}