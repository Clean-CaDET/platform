using System.Collections.Generic;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;

namespace DataSetExplorer.Core.DataSets.Repository
{
    public interface IInstanceRepository
    {
        Instance Get(int id);
        InstanceDTO GetInstanceWithRelatedInstances(int id);
        IEnumerable<Instance> GetInstancesAnnotatedByAnnotator(int projectId, int? annotatorId);
        IEnumerable<Instance> GetAnnotatedInstances(int projectId);
        void Update(Instance instance);
        Instance GetInstanceWithAnnotations(int id);
    }
}
