using System.Collections.Generic;

namespace DataSetExplorer.DataSets.Model.Repository
{
    public interface IDataSetProjectRepository
    {
        DataSetProject GetDataSetProject(int id);
        IEnumerable<DataSetProject> GetDataSetProjects(IEnumerable<int> projectIds);
        void Update(DataSetProject dataSetProject);
    }
}
