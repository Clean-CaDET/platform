using CodeModel.CaDETModel.CodeItems;
using SmellDetector.SmellModel.Reports;
using System.Collections.Generic;

namespace SmellDetector.Detectors
{
    public interface IDetector
    {
        public PartialSmellDetectionReport FindIssues(List<CaDETClass> classes);

    }
}
