using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.DataSetBuilder.Model.Repository
{
    public interface IDataSetRepository
    {
        void Create(DataSet dataSet);
        DataSet GetDataSet(int id);
        IEnumerable<DataSet> GetAll();
        void Update(DataSet dataSet);
        Dictionary<string, List<string>> GetDataSetCodeSmells(int id);
    }
}
