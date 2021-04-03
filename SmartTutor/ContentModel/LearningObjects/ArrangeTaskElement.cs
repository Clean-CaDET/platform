using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.LearningObjects
{
    public class ArrangeTaskElement
    {
        [Key] public int Id { get; set; }
        public int ArrangeTaskContainerId { get; set; }
        public string Text { get; set; }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ArrangeTaskElement);
        }

        public bool Equals(ArrangeTaskElement element)
        {
            return element != null && Id.Equals(element.Id);
        }
    }
}