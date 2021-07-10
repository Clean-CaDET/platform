using DataSetExplorer.DataSetSerializer.ViewModel;
using FluentResults;

namespace DataSetExplorer
{
    public interface IDataSetCreationService
    {
        public Result<string> CreateDataSetSpreadsheet(string basePath, string projectName, string projectAndCommitUrl);
        public Result<string> CreateDataSetSpreadsheet(string basePath, string projectName, string projectAndCommitUrl, NewSpreadSheetColumnModel columnModel);
    }
}
