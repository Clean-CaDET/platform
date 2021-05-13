using SmellDetector.Detectors;
using System;

namespace SmartTutor.QualityAnalysis
{
    public class CodeQualityAnalyzerService: ICodeQualityAnalyzerService
    {
        private IDetector _smellDetector;
        public CodeQualityAnalyzerService(IDetector smellDetector)
        {
            _smellDetector = smellDetector;
        }
        public CodeQualityEvaluation EvaluateCode(CodeSubmission submittedCode)
        {
            throw new NotImplementedException();
        }
    }
}
