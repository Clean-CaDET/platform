using System.Collections.Generic;
using DataSetExplorer.Core.DataSets.Model;

namespace DataSetExplorer.Core.DataSets.Repository
{
    public interface IDataSetInstanceRepository
    {
        Instance GetDataSetInstance(int id);
        IEnumerable<Instance> GetInstancesAnnotatedByAnnotator(int projectId, int? annotatorId);
        IEnumerable<Instance> GetAnnotatedInstances(int projectId);
        void Update(Instance instance);
    }
}
