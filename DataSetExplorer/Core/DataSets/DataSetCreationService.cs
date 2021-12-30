using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CodeModel.CodeParsers.CSharp.Exceptions;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.Core.DataSets.Repository;
using DataSetExplorer.Core.DataSetSerializer;
using DataSetExplorer.Core.DataSetSerializer.ViewModel;
using DataSetExplorer.Infrastructure.RepositoryAdapters;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using FluentResults;
using LibGit2Sharp;

namespace DataSetExplorer.Core.DataSets
{
    public class DataSetCreationService : IDataSetCreationService
    {
        private readonly ICodeRepository _codeRepository;
        private readonly IDataSetRepository _dataSetRepository;
        private readonly IDataSetProjectRepository _dataSetProjectRepository;

        public DataSetCreationService(ICodeRepository codeRepository, IDataSetRepository dataSetRepository, IDataSetProjectRepository dataSetProjectRepository)
        {
            _codeRepository = codeRepository;
            _dataSetRepository = dataSetRepository;
            _dataSetProjectRepository = dataSetProjectRepository;
        }

        public Result<DataSet> CreateEmptyDataSet(string dataSetName, List<CodeSmell> codeSmells)
        {
            var dataSet = new DataSet(dataSetName, codeSmells);
            _dataSetRepository.Create(dataSet);
            return Result.Ok(dataSet);
        }

        public Result<DataSet> AddProjectToDataSet(int dataSetId, string basePath, DataSetProject project, List<SmellFilter> smellFilters, ProjectBuildSettingsDTO projectBuildSettings)
        {
            var initialDataSet = _dataSetRepository.GetDataSet(dataSetId);
            if (initialDataSet == default) return Result.Fail($"DataSet with id: {dataSetId} does not exist.");
            
            Task.Run(() => ProcessInitialDataSetProject(basePath, project, initialDataSet.SupportedCodeSmells, smellFilters, projectBuildSettings));
            initialDataSet.AddProject(project);
            
            _dataSetRepository.Update(initialDataSet);
            return Result.Ok(initialDataSet);
        }

        public Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, IDictionary<string, string> projects, List<CodeSmell> codeSmells)
        {
            return CreateDataSetSpreadsheet(dataSetName, basePath, projects, codeSmells, new NewSpreadSheetColumnModel());
        }

        public Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, IDictionary<string, string> projects, List<CodeSmell> codeSmells, NewSpreadSheetColumnModel columnModel)
        {
            var dataSet = new DataSet(dataSetName, codeSmells);
            foreach(var projectName in projects.Keys)
            {
                // TODO: Update console app and send metrics thresholds to CreateDataSetProject method
                var dataSetProject = CreateDataSetProject(basePath, projectName, projects[projectName], codeSmells, null, null);
                dataSet.AddProject(dataSetProject);
            }

            var excelFileName = ExportToExcel(basePath, dataSetName, columnModel, dataSet);
            return Result.Ok("Data set created: " + excelFileName);
        }

        public Result<DataSet> GetDataSet(int id)
        {
            var dataSet = _dataSetRepository.GetDataSet(id);
            if (dataSet == default) return Result.Fail($"DataSet with id: {id} does not exist.");
            return Result.Ok(dataSet);
        }

        public Result<IEnumerable<DataSet>> GetAllDataSets()
        {
            var dataSets = _dataSetRepository.GetAll();
            return Result.Ok(dataSets);
        }

        public Result<DataSetProject> GetDataSetProject(int id)
        {
            var project = _dataSetProjectRepository.GetDataSetProject(id);
            if (project == default) return Result.Fail($"DataSetProject with id: {id} does not exist.");
            return Result.Ok(project);
        }

        private DataSetProject CreateDataSetProject(string basePath, string projectName, string projectAndCommitUrl, List<CodeSmell> codeSmells, List<SmellFilter> smellFilters, ProjectBuildSettingsDTO projectBuildSettings)
        {
            var gitFolderPath = basePath + projectName + Path.DirectorySeparatorChar + "git";
            _codeRepository.SetupRepository(projectAndCommitUrl, gitFolderPath);
            return CreateDataSetProjectFromRepository(projectAndCommitUrl, projectName, gitFolderPath, codeSmells, smellFilters, projectBuildSettings);
        }

        private static DataSetProject CreateDataSetProjectFromRepository(string projectAndCommitUrl, string projectName, string projectPath, List<CodeSmell> codeSmells, List<SmellFilter> smellFilters, ProjectBuildSettingsDTO projectBuildSettings)
        {
            //TODO: Introduce Director as a separate class and insert through DI.
            var builder = new CaDETToDataSetProjectBuilder(new InstanceFilter(smellFilters), projectAndCommitUrl, projectName, projectPath, codeSmells);
            if (projectBuildSettings.RandomizeClassSelection) builder = builder.RandomizeClassSelection();
            if (projectBuildSettings.RandomizeMemberSelection) builder = builder.RandomizeMemberSelection();
            if (projectBuildSettings.NumOfInstancesType.Equals("Percentage")) return builder.SetProjectExtractionPercentile(projectBuildSettings.NumOfInstances).Build();
            return builder.SetProjectInstancesExtractionNumber(projectBuildSettings.NumOfInstances).Build();
        }

        private void ProcessInitialDataSetProject(string basePath, DataSetProject initialProject, List<CodeSmell> codeSmells, List<SmellFilter> smellFilters, ProjectBuildSettingsDTO projectBuildSettings)
        {
            try
            {
                var project = CreateDataSetProject(basePath, initialProject.Name, initialProject.Url, codeSmells, smellFilters, projectBuildSettings);
                initialProject.CandidateInstances = project.CandidateInstances;
                initialProject.Processed();
                _dataSetProjectRepository.Update(initialProject);
            }
            catch (Exception e) when (e is LibGit2SharpException || e is NonUniqueFullNameException)
            {
                initialProject.Failed();
                _dataSetProjectRepository.Update(initialProject);
            }
        }

        private string ExportToExcel(string basePath, string projectName, NewSpreadSheetColumnModel columnModel, DataSet dataSet)
        {
            var sheetFolderPath = basePath + projectName + Path.DirectorySeparatorChar + "sheets" + Path.DirectorySeparatorChar;
            if(!Directory.Exists(sheetFolderPath)) Directory.CreateDirectory(sheetFolderPath);
            var exporter = new NewDataSetExporter(sheetFolderPath, columnModel);

            var fileName = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
            exporter.Export(dataSet, fileName);
            return fileName;
        }

        public Result<Dictionary<string, List<string>>> GetDataSetCodeSmells(int id)
        {
            var codeSmells = _dataSetRepository.GetDataSetCodeSmells(id);
            if (codeSmells == default) return Result.Fail($"DataSet with id: {id} does not exist.");
            return Result.Ok(codeSmells);
        }

        public Result<DataSet> DeleteDataSet(int id)
        {
            var dataset = _dataSetRepository.DeleteDataSet(id);
            return Result.Ok(dataset);
        }

        public Result<DataSet> UpdateDataSet(DataSet dataset)
        {
            var updatedDataset = _dataSetRepository.UpdateDataSet(dataset);
            return Result.Ok(updatedDataset);
        }

        public Result<DataSetProject> DeleteDataSetProject(int id)
        {
            var project = _dataSetRepository.DeleteDataSetProject(id);
            return Result.Ok(project);
        }

        public Result<DataSetProject> UpdateDataSetProject(DataSetProject project)
        {
            var updatedProject = _dataSetRepository.UpdateDataSetProject(project);
            return Result.Ok(updatedProject);
        }
    }
}
