using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;

namespace SmellDetector.Detectors
{
    public interface IDetector
    {
        public PartialSmellDetectionReport FindIssues(CaDETClassDTO caDetClassDto);

    }
}
