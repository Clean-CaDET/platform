using System.Collections.Generic;

namespace DataSetExplorer.DataSets.Model.Repository
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
