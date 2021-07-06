using FluentResults;

namespace DataSetExplorer
{
    interface IAnnotationConsistencyService
    {
        public Result<string> CheckMetricsSignificanceBetweenAnnotatorsForSeverity(int severity);
        public Result<string> CheckMetricsSignificanceInAnnotationsForAnnotator(int annotatorId);
        public Result<string> CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(int severity);
        public Result<string> CheckAnnotationConsistencyForAnnotator(int annotatorId);
    }
}
