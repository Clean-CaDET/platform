namespace SmellDetector.SmellModel
{
    public class Issue
    {
        public SmellType IssueType { get; set; }

        public string Problem { get; set; }

        public Issue()
        {
            IssueType = SmellType.WITHOUT_BAD_SMELL;
            Problem = "Problem is: ";
        }
    }
}
