using System.Collections.Generic;
using SmellDetector.Detectors;
using SmellDetector.Detectors.RuleEngines;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;

namespace SmellDetector.Services
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
                new LongMethodRuleEngine(),
                new LongParameterListRuleEngine(),
            };
        }

        public SmellDetectionReport GenerateSmellDetectionReport(CaDETClassDTO caDetClassDto)
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



