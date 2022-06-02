namespace DataSetExplorer.Core.Annotations.Model
{
    public class SmellHeuristic
    {
        public int Id { get; private set; }
        public string Description { get; set; }
        public bool IsApplicable { get; private set; }
        public string ReasonForApplicability { get; private set; }

        internal SmellHeuristic(string description, bool isApplicable, string reason)
        {
            Description = description;
            IsApplicable = isApplicable;
            ReasonForApplicability = reason;
        }

        private SmellHeuristic()
        {
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
