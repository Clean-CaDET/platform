using DataSetExplorer.DataSets.Model;
using FluentResults;

namespace DataSetExplorer.Annotations
{
    public interface IDataSetAnnotationService
    {
        Result<Annotation> AddDataSetAnnotation(Annotation annotation, int dataSetInstanceId, int annotatorId);
        Result<Annotation> UpdateAnnotation(Annotation changed, int annotationId, int annotatorId);
    }
}
