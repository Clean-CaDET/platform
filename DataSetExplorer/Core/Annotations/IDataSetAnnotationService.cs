using DataSetExplorer.Core.Annotations.Model;
using FluentResults;

namespace DataSetExplorer.Core.Annotations
{
    public interface IDataSetAnnotationService
    {
        Result<Annotation> AddDataSetAnnotation(Annotation annotation, int dataSetInstanceId, int annotatorId);
        Result<Annotation> UpdateAnnotation(Annotation changed, int annotationId, int annotatorId);
    }
}
