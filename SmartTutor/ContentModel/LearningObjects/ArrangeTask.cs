using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTutor.ContentModel.LearningObjects
{
    [Table("ArrangeTasks")]
    public class ArrangeTask : LearningObject
    {
        public string Text { get; set; }
        public List<ArrangeTaskContainer> Containers { get; set; }
    }
}