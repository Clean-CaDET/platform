namespace DataSetExplorer.DataSetBuilder.Model
{
    public class SmellHeuristic
    {
        public string Description { get; set; }
        public bool IsApplicable { get; }
        public string ReasonForApplicability { get; }

        internal SmellHeuristic(string description, bool isApplicable, string reason)
        {
            Description = description;
            IsApplicable = isApplicable;
            ReasonForApplicability = reason;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
