namespace DataSetExplorer
{
    interface IAnnotationConsistencyChecker
    {
        public void CheckMetricsSignificanceBetweenAnnotatorsForSeverity(int severity);
        public void CheckMetricsSignificanceInAnnotationsForAnnotator(int annotatorId);
        public void CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(int severity);
        public void CheckAnnotationConsistencyForAnnotator(int annotatorId);
    }
}
