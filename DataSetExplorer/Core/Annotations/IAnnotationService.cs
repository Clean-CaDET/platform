using DataSetExplorer.Core.Annotations.Model;
using FluentResults;

namespace DataSetExplorer.Core.Annotations
{
    public interface IAnnotationService
    {
        Result<Annotation> AddAnnotation(Annotation annotation, int instanceId, int annotatorId);
        Result<Annotation> UpdateAnnotation(Annotation changed, int annotationId, int annotatorId);
    }
}
