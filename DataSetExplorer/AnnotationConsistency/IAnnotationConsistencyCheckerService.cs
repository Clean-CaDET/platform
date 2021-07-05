using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer
{
    interface IAnnotationConsistencyCheckerService
    {
        public Result<Dictionary<string, Dictionary<string, string>>> CheckMetricsSignificanceBetweenAnnotatorsForSeverity(int severity);
        public Result<Dictionary<string, Dictionary<string, string>>> CheckMetricsSignificanceInAnnotationsForAnnotator(int annotatorId);
        public Result<Dictionary<string, string>> CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(int severity);
        public Result<Dictionary<string, string>> CheckAnnotationConsistencyForAnnotator(int annotatorId);
    }
}
