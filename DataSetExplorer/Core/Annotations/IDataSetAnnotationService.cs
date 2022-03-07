using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using FluentResults;

namespace DataSetExplorer.Core.Annotations
{
    public interface IDataSetAnnotationService
    {
        Result<Annotation> AddDataSetAnnotation(Annotation annotation, int dataSetInstanceId, int annotatorId);
        Result<Annotation> UpdateAnnotation(Annotation changed, int annotationId, int annotatorId);
        Result<InstanceDTO> GetInstanceWithRelatedInstances(int id);
        Result<Instance> GetInstanceWithAnnotations(int id);
    }
}
