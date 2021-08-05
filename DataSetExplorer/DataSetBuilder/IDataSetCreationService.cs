using DataSetExplorer.DataSetSerializer.ViewModel;
using FluentResults;
using System.Collections.Specialized;

namespace DataSetExplorer
{
    public interface IDataSetCreationService
    {
        public Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, ListDictionary projects);
        public Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, ListDictionary projects, NewSpreadSheetColumnModel columnModel);
    }
}
