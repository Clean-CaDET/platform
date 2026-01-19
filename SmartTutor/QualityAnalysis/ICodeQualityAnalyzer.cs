namespace SmartTutor.QualityAnalysis
{
    public interface ICodeQualityAnalyzer
    {
        public CodeEvaluation EvaluateCode(CodeSubmission submission);
    }
}
