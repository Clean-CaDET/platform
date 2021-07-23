using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.DataSetBuilder.Model.Repository
{
    public interface IDataSetRepository
    {
        public void Create(DataSet dataSet);
        public DataSet GetDataSet(int id);
    }
}
