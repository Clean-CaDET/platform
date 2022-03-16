using System.Collections.Generic;
using DataSetExplorer.Core.DataSets.Model;

namespace DataSetExplorer.Core.DataSets.Repository
{
    public interface IDataSetProjectRepository
    {
        DataSetProject GetDataSetProject(int id);
        IEnumerable<DataSetProject> GetDataSetProjects(IEnumerable<int> projectIds);
        DataSetProject Update(DataSetProject dataSetProject);
        DataSetProject Delete(int id);
    }
}
