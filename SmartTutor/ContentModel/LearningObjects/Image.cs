using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTutor.ContentModel.LearningObjects
{
    [Table("Images")]
    public class Image : LearningObject
    {
        public string Url { get; set; }
        public string Caption { get; set; }
    }
}