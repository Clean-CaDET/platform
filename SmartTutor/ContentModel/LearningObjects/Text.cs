using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTutor.ContentModel.LearningObjects
{
    [Table("Texts")]
    public class Text : LearningObject
    {
        public string Content { get; set; }
    }
}