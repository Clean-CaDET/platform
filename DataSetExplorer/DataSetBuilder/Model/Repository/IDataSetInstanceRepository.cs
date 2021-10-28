using System.Collections.Generic;

namespace DataSetExplorer.DataSetBuilder.Model.Repository
{
    public interface IDataSetInstanceRepository
    {
        Instance GetDataSetInstance(int id);
        IEnumerable<Instance> GetInstancesAnnotatedByAnnotator(int projectId, int? annotatorId);
        IEnumerable<Instance> GetAnnotatedInstances(int projectId);
        void Update(Instance instance);
    }
}
