using System.Collections.Generic;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;

namespace SmellDetector.Detectors
{
    public interface IDetector
    {
        public PartialSmellDetectionReport FindIssues(List<CaDETClassDTO> caDetClassDto);

    }
}
