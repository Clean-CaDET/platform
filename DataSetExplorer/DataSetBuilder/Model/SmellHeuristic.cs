namespace DataSetExplorer.DataSetBuilder.Model
{
    public class SmellHeuristic
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
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
