using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.AnnotationSchema.Model;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer.Core.Annotations
{
    public interface IAnnotationService
    {
        Result<Annotation> AddAnnotation(Annotation annotation, int instanceId, int annotatorId);
        Result<Annotation> UpdateAnnotation(Annotation changed, int annotationId, int annotatorId);
        Result UpdateAnnotationsAfterHeuristicDeletion(CodeSmellDefinition codeSmellDefinition, HeuristicDefinition heuristic);
        Result UpdateAnnotationsAfterSeverityDeletion(CodeSmellDefinition codeSmellDefinition, SeverityDefinition severity);
        Result UpdateAnnotationsAfterSeverityUpdate(CodeSmellDefinition codeSmellDefinition, SeverityDefinition severity, SeverityDefinition oldSeverity);
        Result UpdateAnnotationsAfterHeuristicUpdate(CodeSmellDefinition codeSmellDefinition, HeuristicDefinition heuristic, HeuristicDefinition oldHeuristic);
        Result<List<CodeSmell>> GetCodeSmellsByDefinition(CodeSmellDefinition codeSmellDefinition);
        Result<CodeSmell> UpdateCodeSmell(CodeSmell codeSmell);
    }
}
