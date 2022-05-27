using System.Collections.Generic;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.AnnotationSchema.Model;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.Core.DataSetSerializer.ViewModel;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using DataSetExplorer.UI.Controllers.Dataset.DTOs.Summary;
using FluentResults;

namespace DataSetExplorer.Core.DataSets
{
    public interface IDataSetCreationService
    {
        Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, IDictionary<string, string> projects, List<CodeSmell> codeSmells);
        Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, IDictionary<string, string> projects, List<CodeSmell> codeSmells, NewSpreadSheetColumnModel columnModel);
        Result<DataSet> CreateEmptyDataSet(string dataSetName, List<CodeSmell> codeSmells);
        Result<DataSetProject> AddProjectToDataSet(int dataSetId, string basePath, DataSetProject project, List<SmellFilter> smellFilters, ProjectBuildSettingsDTO projectBuildSettings);
        Result<DatasetDetailDTO> GetDataSet(int id);
        Result<DataSet> GetDataSetForExport(int id);
        Result<IEnumerable<DatasetSummaryDTO>> GetAllDataSets();
        Result<DataSetProject> GetDataSetProject(int id);
        Result<List<CodeSmell>> GetDataSetCodeSmells(int id);
        Result<DataSet> DeleteDataSet(int id);
        Result<DataSet> UpdateDataSet(DataSet dataset);
        Result<DataSetProject> DeleteDataSetProject(int id);
        Result<DataSetProject> UpdateDataSetProject(DataSetProject project);
    }
}
