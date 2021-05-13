namespace SmartTutor.QualityAnalysis
{
    public interface ICodeQualityAnalyzerService
    {
        public CodeQualityEvaluation EvaluateCode(CodeSubmission submittedCode);
    }
}
