namespace DataSetExplorer.Core.AnnotationSchema.Model
{
    public class HeuristicDefinition
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public HeuristicDefinition(string name, string description)
        {
            Name = name;
            Description = description;
        }

        private HeuristicDefinition()
        {
        }

        public void Update(HeuristicDefinition other)
        {
            Name = other.Name;
            Description = other.Description;
        }
    }
}