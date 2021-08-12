using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.DataSetBuilder.Model.Repository
{
    public interface IDataSetProjectRepository
    {
        DataSetProject GetDataSetProject(int id);
        void Update(DataSetProject dataSetProject);
    }
}
