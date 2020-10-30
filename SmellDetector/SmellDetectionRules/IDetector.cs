using System.Collections.Generic;
using SmellDetector.Controllers;
using SmellDetector.SmellModel;

namespace SmellDetector.SmellDetectionRules
{
    public interface IDetector
    {
        public PartialSmellDetectionReport findIssues(CaDETClassDTO caDetClassDto);

    }
}
