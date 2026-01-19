namespace DataSetExplorer.Core.AnnotationSchema.Model
{
    public class SeverityDefinition
    {
        public int Id { get; private set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public SeverityDefinition(string value, string description)
        {
            Value = value;
            Description = description;
        }

        private SeverityDefinition()
        {
        }

        public void Update(SeverityDefinition other)
        {
            Value = other.Value;
            Description = other.Description;
        }
    }
}