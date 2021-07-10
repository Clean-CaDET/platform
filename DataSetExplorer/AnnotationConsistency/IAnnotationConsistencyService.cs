using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace DataSetExplorer
{
    interface IAnnotationConsistencyService
    {
        public Result<string> CheckMetricsSignificanceBetweenAnnotatorsForSeverity(int severity, ListDictionary projects, List<Annotator> annotators);
        public Result<string> CheckMetricsSignificanceInAnnotationsForAnnotator(int annotatorId, ListDictionary projects, List<Annotator> annotators);
        public Result<string> CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(int severity, ListDictionary projects, List<Annotator> annotators);
        public Result<string> CheckAnnotationConsistencyForAnnotator(int annotatorId, ListDictionary projects, List<Annotator> annotators);
    }
}
