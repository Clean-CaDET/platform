using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using FluentResults;

namespace DataSetExplorer.Core.Annotations
{
    public interface IAnnotationService
    {
        Result<Annotation> AddAnnotation(Annotation annotation, int instanceId, int annotatorId);
        Result<Annotation> UpdateAnnotation(Annotation changed, int annotationId, int annotatorId);
        Result<InstanceDTO> GetInstanceWithRelatedInstances(int projectId, int id);
        Result<Instance> GetInstanceWithAnnotations(int id);
    }
}
