using System.Collections.Generic;

namespace DataSetExplorer.DataSetBuilder.Model.Repository
{
    public interface IDataSetInstanceRepository
    {
        DataSetInstance GetDataSetInstance(int id);
        IEnumerable<DataSetInstance> GetInstancesAnnotatedByAnnotator(int projectId, int? annotatorId);
        IEnumerable<DataSetInstance> GetAnnotatedInstances(int projectId);
        void Update(DataSetInstance instance);
    }
}
