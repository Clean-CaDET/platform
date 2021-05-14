using CodeModel.CaDETModel.CodeItems;
using SmellDetector.Detectors;
using SmellDetector.Detectors.RuleEngines;
using SmellDetector.SmellModel.Reports;
using System.Collections.Generic;

namespace SmellDetector
{
    public class DetectionService
    {
        public List<IDetector> Detectors { get; set; }

        public DetectionService()
        {
            LoadDetectors();
        }

        private void LoadDetectors()
        {
            Detectors = new List<IDetector>
            {
                new ClassMetricRuleEngine()
            };
        }

        public SmellDetectionReport GenerateSmellDetectionReport(List<CaDETClass> caDetClassDto)
        {
            SmellDetectionReport smellDetectionReport = new SmellDetectionReport();

            foreach (IDetector detector in Detectors)
            {
                smellDetectionReport.AddPartialReport(detector.FindIssues(caDetClassDto));
            }
            
            return smellDetectionReport;
        }

    }
}



