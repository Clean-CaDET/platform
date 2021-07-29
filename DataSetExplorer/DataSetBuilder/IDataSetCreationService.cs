using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer.ViewModel;
using FluentResults;

namespace DataSetExplorer
{
    public interface IDataSetCreationService
    {
        public Result<DataSet> CreateDataSetInDatabase(string basePath, string projectName, string projectAndCommitUrl);
        public Result<string> CreateDataSetSpreadsheet(string basePath, string projectName, string projectAndCommitUrl);
        public Result<string> CreateDataSetSpreadsheet(string basePath, string projectName, NewSpreadSheetColumnModel columnModel, DataSet dataSet);
        public Result<DataSet> GetDataSetIfCreated(int id);
    }
}
