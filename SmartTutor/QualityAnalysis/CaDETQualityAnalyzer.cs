using SmellDetector.Detectors;

namespace SmartTutor.QualityAnalysis
{
    public class CaDETQualityAnalyzer: ICodeQualityAnalyzer
    {
        private readonly IDetector _smellDetector;
        public CaDETQualityAnalyzer(IDetector smellDetector)
        {
            _smellDetector = smellDetector;
        }
        public CodeQualityEvaluation EvaluateCode(CodeSubmission submittedCode)
        {
            var issues = _smellDetector.FindIssues(submittedCode.);
        }
    }
}
