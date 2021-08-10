using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer.ViewModel;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer
{
    public interface IDataSetCreationService
    {
        public Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, IDictionary<string, string> projects);
        public Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, IDictionary<string, string> projects, NewSpreadSheetColumnModel columnModel);
        public Result<DataSet> CreateDataSetInDatabase(string dataSetName, string basePath, IDictionary<string, string> projects);
        public Result<DataSet> GetDataSet(int id);
    }
}
