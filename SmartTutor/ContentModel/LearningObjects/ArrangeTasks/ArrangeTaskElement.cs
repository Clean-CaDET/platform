namespace SmartTutor.ContentModel.LearningObjects.ArrangeTasks
{
    public class ArrangeTaskElement
    {
        public int Id { get; private set; }
        public int ArrangeTaskContainerId { get; private set; }
        public string Text { get; private set; }

        public ArrangeTaskElement(int id, int arrangeTaskContainerId, string text)
        {
            Id = id;
            ArrangeTaskContainerId = arrangeTaskContainerId;
            Text = text;
        }

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