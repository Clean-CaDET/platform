using System.Collections.Generic;
using DataSetExplorer.Core.DataSets.Model;

namespace DataSetExplorer.Core.DataSets.Repository
{
    public interface IDataSetRepository
    {
        void Create(DataSet dataSet);
        DataSet GetDataSet(int id);
        IEnumerable<DataSet> GetAll();
        void Update(DataSet dataSet);
        Dictionary<string, List<string>> GetDataSetCodeSmells(int id);
        DataSet DeleteDataSet(int id);
        DataSet UpdateDataSet(DataSet dataset);
        DataSetProject DeleteDataSetProject(int id);
        DataSetProject UpdateDataSetProject(DataSetProject project);
    }
}
