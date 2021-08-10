using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer
{
    interface IAnnotationConsistencyService
    {
        public Result<string> CheckMetricsSignificanceBetweenAnnotatorsForSeverity(int severity, IDictionary<string, string> projects, List<Annotator> annotators);
        public Result<string> CheckMetricsSignificanceInAnnotationsForAnnotator(int annotatorId, IDictionary<string, string> projects, List<Annotator> annotators);
        public Result<string> CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(int severity, IDictionary<string, string> projects, List<Annotator> annotators);
        public Result<string> CheckAnnotationConsistencyForAnnotator(int annotatorId, IDictionary<string, string> projects, List<Annotator> annotators);
    }
}
