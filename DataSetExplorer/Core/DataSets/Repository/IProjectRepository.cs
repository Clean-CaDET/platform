using System.Collections.Generic;
using DataSetExplorer.Core.DataSets.Model;

namespace DataSetExplorer.Core.DataSets.Repository
{
    public interface IProjectRepository
    {
        DataSetProject Get(int id);
        IEnumerable<DataSetProject> GetAll(IEnumerable<int> projectIds);
        DataSetProject Update(DataSetProject dataSetProject);
        DataSetProject Delete(int id);
        DataSetProject GetProjectWithGraphInstances(int id);
        List<DataSetProject> GetAllByDatasetId(int datasetId);
    }
}
