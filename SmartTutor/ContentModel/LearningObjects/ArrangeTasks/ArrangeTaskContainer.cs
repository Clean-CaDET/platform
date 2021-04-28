using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.ArrangeTasks
{
    public class ArrangeTaskContainer
    {
        public int Id { get; private set; }
        public int ArrangeTaskId { get; private set; }
        public string Title { get; private set; }
        public List<ArrangeTaskElement> Elements { get; private set; }

        private ArrangeTaskContainer() {}
        public ArrangeTaskContainer(int id, int arrangeTaskId, string title, List<ArrangeTaskElement> elements): this()
        {
            Id = id;
            ArrangeTaskId = arrangeTaskId;
            Title = title;
            Elements = elements;
        }

        public bool IsCorrectSubmission(List<int> elementIds)
        {
            return elementIds.Count == Elements.Count
                   && Elements.Select(e => e.Id).All(elementIds.Contains);
        }
    }
}