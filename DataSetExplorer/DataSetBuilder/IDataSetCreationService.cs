using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer.ViewModel;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer
{
    public interface IDataSetCreationService
    {
        Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, IDictionary<string, string> projects, List<CodeSmell> codeSmells);
        Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, IDictionary<string, string> projects, List<CodeSmell> codeSmells, NewSpreadSheetColumnModel columnModel);
        Result<DataSet> CreateEmptyDataSet(string dataSetName, List<CodeSmell> codeSmells);
        Result<DataSet> AddProjectToDataSet(int dataSetId, string basePath, DataSetProject project, List<SmellFilter> smellFilters);
        Result<DataSet> GetDataSet(int id);
        Result<IEnumerable<DataSet>> GetAllDataSets();
        Result<DataSetProject> GetDataSetProject(int id);
        Result<Dictionary<string, List<string>>> GetDataSetCodeSmells(int id);
        Result<DataSet> DeleteDataSet(int id);
        Result<DataSet> UpdateDataSet(DataSet dataset);
        Result<DataSetProject> DeleteDataSetProject(int id);
        Result<DataSetProject> UpdateDataSetProject(DataSetProject project);
    }
}
