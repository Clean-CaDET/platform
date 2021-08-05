using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer.ViewModel;
using FluentResults;
using System.Collections.Specialized;

namespace DataSetExplorer
{
    public interface IDataSetCreationService
    {
        public Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, ListDictionary projects);
        public Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, ListDictionary projects, NewSpreadSheetColumnModel columnModel);
        public Result<DataSet> CreateDataSetInDatabase(string dataSetName, string basePath, ListDictionary projects);
        public Result<DataSet> GetDataSet(int id);
    }
}
