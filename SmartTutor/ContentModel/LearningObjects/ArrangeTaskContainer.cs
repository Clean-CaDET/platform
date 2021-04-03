using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects
{
    public class ArrangeTaskContainer
    {
        [Key] public int Id { get; set; }
        public int ArrangeTaskId { get; set; }
        public string Title { get; set; }
        public List<ArrangeTaskElement> Elements { get; set; }

        public bool IsCorrectSubmission(ArrangeTaskContainer submittedContainer)
        {
            var submittedElements = submittedContainer.Elements;
            return submittedElements.Count == Elements.Count
                   && Elements.All(submittedElements.Contains);
        }
    }
}