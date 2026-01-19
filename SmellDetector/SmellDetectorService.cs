using CodeModel;
using SmellDetector.Detectors;
using SmellDetector.Detectors.RuleEngines;
using SmellDetector.SmellModel.Reports;
using System.Collections.Generic;

namespace SmellDetector
{
    public class SmellDetectorService
    {
        private readonly CodeModelFactory _codeModelFactory;
        private readonly List<IDetector> _detectors;

        public SmellDetectorService()
        {
            _codeModelFactory = new CodeModelFactory();
            _detectors = new List<IDetector>
            {
                new ClassMetricRuleEngine()
            };
        }

        public SmellDetectionReport AnalyzeCodeQuality(string[] sourceCode)
        {
            var project = _codeModelFactory.CreateProject(sourceCode);
            var smellDetectionReport = new SmellDetectionReport();

            foreach (var detector in _detectors)
            {
                smellDetectionReport.AddPartialReport(detector.FindIssues(project.Classes));
            }
            
            return smellDetectionReport;
        }

    }
}



