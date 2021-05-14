namespace SmartTutor.QualityAnalysis
{
    public interface ICodeQualityAnalyzer
    {
        public CodeQualityEvaluation EvaluateCode(CodeSubmission submission);
    }
}
