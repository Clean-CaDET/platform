using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.DataSetBuilder.Model.Repository
{
    public interface IDataSetProjectRepository
    {
        DataSetProject GetDataSetProject(int id);
        IEnumerable<DataSetProject> GetDataSetProjects(IEnumerable<int> projectIds);
        void Update(DataSetProject dataSetProject);
    }
}
