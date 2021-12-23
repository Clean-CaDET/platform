using System.Collections.Generic;
using DataSetExplorer.Core.Annotations.Model;
using FluentResults;

namespace DataSetExplorer.Core.AnnotationConsistency
{
    public interface IAnnotationConsistencyService
    {
        Result<string> CheckMetricsSignificanceBetweenAnnotatorsForSeverity(int severity, IDictionary<string, string> projects, List<Annotator> annotators);
        Result<string> CheckMetricsSignificanceInAnnotationsForAnnotator(int annotatorId, IDictionary<string, string> projects, List<Annotator> annotators);
        Result<string> CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(int severity, IDictionary<string, string> projects, List<Annotator> annotators);
        Result<string> CheckAnnotationConsistencyForAnnotator(int annotatorId, IDictionary<string, string> projects, List<Annotator> annotators);

        Result<Dictionary<string, string>> CheckAnnotationConsistencyForAnnotator(int projectId, int annotatorId);
        Result<Dictionary<string, string>> CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(int projectId, int severity);
        Result<Dictionary<string, Dictionary<string, string>>> CheckMetricsSignificanceInAnnotationsForAnnotator(int projectId, int annotatorId);
        Result<Dictionary<string, Dictionary<string, string>>> CheckMetricsSignificanceBetweenAnnotatorsForSeverity(int projectId, int severity);
    }
}
