using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer.ViewModel;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer
{
    public interface IDataSetCreationService
    {
        Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, IDictionary<string, string> projects);
        Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, IDictionary<string, string> projects, NewSpreadSheetColumnModel columnModel);
        Result<DataSet> CreateEmptyDataSet(string dataSetName);
        Result<DataSet> AddProjectsToDataSet(int dataSetId, string basePath, IEnumerable<DataSetProject> projects);
        Result<DataSet> GetDataSet(int id);
        Result<DataSetProject> GetDataSetProject(int id);
    }
}
