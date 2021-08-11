using CodeModel.CodeParsers.CSharp.Exceptions;
using DataSetExplorer.DataSetBuilder;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetBuilder.Model.Repository;
using DataSetExplorer.DataSetSerializer;
using DataSetExplorer.DataSetSerializer.ViewModel;
using DataSetExplorer.RepositoryAdapters;
using FluentResults;
using LibGit2Sharp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public Result<DataSet> CreateEmptyDataSet(string dataSetName)
        {
            var dataSet = new DataSet(dataSetName);
            _dataSetRepository.Create(dataSet);
            return Result.Ok(dataSet);
        }

        public Result<DataSet> AddProjectsToDataSet(int dataSetId, string basePath, IEnumerable<DataSetProject> projects)
        {
            var initialDataSet = _dataSetRepository.GetDataSet(dataSetId);
            if (initialDataSet == default) return Result.Fail($"DataSet with id: {dataSetId} does not exist.");
            foreach (var project in projects)
            {
                Task.Run(() => ProcessInitialDataSetProject(basePath, project));
                initialDataSet.AddProject(project);
            }
            _dataSetRepository.Update(initialDataSet);
            return Result.Ok(initialDataSet);
        }

        public Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, IDictionary<string, string> projects)
        {
            return CreateDataSetSpreadsheet(dataSetName, basePath, projects, new NewSpreadSheetColumnModel());
        }

        public Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, IDictionary<string, string> projects, NewSpreadSheetColumnModel columnModel)
        {
            //TODO: Once we establish some DB, we can have the export to excel operation be separate from the "CreateDataSet"
            var dataSet = new DataSet(dataSetName);
            foreach(var projectName in projects.Keys)
            {
                var dataSetProject = CreateDataSetProject(basePath, projectName, projects[projectName]);
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

        private DataSetProject CreateDataSetProject(string basePath, string projectName, string projectAndCommitUrl)
        {
            var gitFolderPath = basePath + projectName + Path.DirectorySeparatorChar + "git";
            _codeRepository.SetupRepository(projectAndCommitUrl, gitFolderPath);
            return CreateDataSetProjectFromRepository(projectAndCommitUrl, projectName, gitFolderPath);
        }

        private static DataSetProject CreateDataSetProjectFromRepository(string projectAndCommitUrl, string projectName, string projectPath)
        {
            //TODO: Introduce Director as a separate class and insert through DI.
            var builder = new CaDETToDataSetProjectBuilder(projectAndCommitUrl, projectName, projectPath);
            return builder.IncludeMembersWith(10).IncludeClassesWith(3, 5)
                .RandomizeClassSelection().RandomizeMemberSelection()
                .SetProjectExtractionPercentile(10).Build();
        }

        private void ProcessInitialDataSetProject(string basePath, DataSetProject initialProject)
        {
            try
            {
                var project = CreateDataSetProject(basePath, initialProject.Name, initialProject.Url);
                initialProject.AddInstances(project.Instances.ToList());
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
    }
}
