using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.LearningObjects
{
    public class ArrangeTaskContainer
    {
        [Key] public int Id { get; set; }
        public int ArrangeTaskId { get; set; }
        public string Title { get; set; }
        public List<ArrangeTaskElement> CorrectlyArrangedElements { get; set; }
    }
}