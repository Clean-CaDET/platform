using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTutor.ContentModel.LearningObjects
{
    [Table("Videos")]
    public class Video : LearningObject
    {
        public string Url { get; set; }
    }
}