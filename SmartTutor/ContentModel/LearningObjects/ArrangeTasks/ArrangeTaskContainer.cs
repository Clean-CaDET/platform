using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.ArrangeTasks
{
    public class ArrangeTaskContainer
    {
        [Key] public int Id { get; set; }
        public int ArrangeTaskId { get; set; }
        public string Title { get; set; }
        public List<ArrangeTaskElement> Elements { get; set; }

        public bool IsCorrectSubmission(List<int> elementIds)
        {
            return elementIds.Count == Elements.Count
                   && Elements.Select(e => e.Id).All(elementIds.Contains);
        }
    }
}