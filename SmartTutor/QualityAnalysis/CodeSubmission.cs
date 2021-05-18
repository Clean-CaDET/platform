namespace SmartTutor.QualityAnalysis
{
    public class CodeSubmission
    {
        public string[] SourceCode { get; private set; }
        public int LearnerId { get; private set; }

        public CodeSubmission(string[] sourceCode, int learnerId)
        {
            SourceCode = sourceCode;
            LearnerId = learnerId;
        }
    }
}