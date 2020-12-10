namespace PlatformInteractionTool.DataSetBuilder.Model
{
    internal class SmellHeuristic
    {
        public string Description { get; set; }
        public bool IsApplicable { get; set; }
        public string ReasonForApplicability { get; set; }

        internal SmellHeuristic(string description, string reason)
        {
            Description = description;
            IsApplicable = true;
            ReasonForApplicability = reason;
        }
    }
}
