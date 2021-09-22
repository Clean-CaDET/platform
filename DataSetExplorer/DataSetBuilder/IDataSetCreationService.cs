using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer.ViewModel;
using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSetExplorer
{
    public interface IDataSetCreationService
    {
        Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, IDictionary<string, string> projects, List<CodeSmell> codeSmells);
        Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, IDictionary<string, string> projects, List<CodeSmell> codeSmells, NewSpreadSheetColumnModel columnModel);
        Result<DataSet> CreateEmptyDataSet(string dataSetName, List<CodeSmell> codeSmells);
        Result<DataSet> AddProjectsToDataSet(int dataSetId, string basePath, List<DataSetProject> projects);
        Result<DataSet> GetDataSet(int id);
        Result<IEnumerable<DataSet>> GetAllDataSets();
        Result<DataSetProject> GetDataSetProject(int id);
    }
}
