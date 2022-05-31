using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.AnnotationSchema.Model;
using FluentResults;

namespace DataSetExplorer.Core.Annotations
{
    public interface IAnnotationService
    {
        Result<Annotation> AddAnnotation(Annotation annotation, int instanceId, int annotatorId);
        Result<Annotation> UpdateAnnotation(Annotation changed, int annotationId, int annotatorId);
        Result UpdateAnnotationsAfterHeuristicDeletion(CodeSmellDefinition codeSmellDefinition, HeuristicDefinition heuristicId);
    }
}
