using CodeModel.CodeParsers.CSharp.Exceptions;
using DataSetExplorer.DataSetBuilder;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetBuilder.Model.Repository;
using DataSetExplorer.DataSetSerializer;
using DataSetExplorer.DataSetSerializer.ViewModel;
using DataSetExplorer.RepositoryAdapters;
using FluentResults;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DataSetExplorer
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

        public Result<DataSet> AddProjectToDataSet(int dataSetId, string basePath, DataSetProject project)
        {
            var initialDataSet = _dataSetRepository.GetDataSet(dataSetId);
            if (initialDataSet == default) return Result.Fail($"DataSet with id: {dataSetId} does not exist.");
            
            Task.Run(() => ProcessInitialDataSetProject(basePath, project, initialDataSet.SupportedCodeSmells));
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
                var dataSetProject = CreateDataSetProject(basePath, projectName, projects[projectName], codeSmells, null);
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

        private DataSetProject CreateDataSetProject(string basePath, string projectName, string projectAndCommitUrl, List<CodeSmell> codeSmells, List<MetricThresholds> metricsThresholds)
        {
            var gitFolderPath = basePath + projectName + Path.DirectorySeparatorChar + "git";
            _codeRepository.SetupRepository(projectAndCommitUrl, gitFolderPath);
            return CreateDataSetProjectFromRepository(projectAndCommitUrl, projectName, gitFolderPath, codeSmells, metricsThresholds);
        }

        private static DataSetProject CreateDataSetProjectFromRepository(string projectAndCommitUrl, string projectName, string projectPath, List<CodeSmell> codeSmells, List<MetricThresholds> metricsThresholds)
        {
            //TODO: Introduce Director as a separate class and insert through DI.
            var builder = new CaDETToDataSetProjectBuilder(new InstanceFilter(metricsThresholds), projectAndCommitUrl, projectName, projectPath, codeSmells);
            return builder.RandomizeClassSelection().RandomizeMemberSelection()
                .SetProjectExtractionPercentile(10).Build();
        }

        private void ProcessInitialDataSetProject(string basePath, DataSetProject initialProject, List<CodeSmell> codeSmells)
        {
            try
            {
                var project = CreateDataSetProject(basePath, initialProject.Name, initialProject.Url, codeSmells, initialProject.MetricsThresholds);
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
    }
}
