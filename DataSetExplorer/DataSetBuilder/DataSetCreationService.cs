using DataSetExplorer.DataSetBuilder;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetBuilder.Model.Repository;
using DataSetExplorer.DataSetSerializer;
using DataSetExplorer.DataSetSerializer.ViewModel;
using DataSetExplorer.RepositoryAdapters;
using FluentResults;
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

        public DataSetCreationService(ICodeRepository codeRepository, IDataSetRepository dataSetRepository)
        {
            _codeRepository = codeRepository;
            _dataSetRepository = dataSetRepository;
        }

        public Result<DataSet> CreateDataSetInDatabase(string dataSetName, string basePath, IDictionary<string, string> projects)
        {
            var initialDataSet = new DataSet(dataSetName);
            _dataSetRepository.Create(initialDataSet);
            Task.Run(() => ProcessInitialDataSet(basePath, projects, initialDataSet));
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
                var gitFolderPath = basePath + projectName + Path.DirectorySeparatorChar + "git";
                _codeRepository.SetupRepository(projects[projectName], gitFolderPath);
                var dataSetProject = CreateDataSetProjectFromRepository(projects[projectName], projectName, gitFolderPath);
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

        private void ProcessInitialDataSet(string basePath, IDictionary<string, string> projects, DataSet initialDataSet)
        {
            foreach (var projectName in projects.Keys)
            {
                var project = CreateDataSetProject(basePath, projectName, projects[projectName]);
                project.Processed();
                initialDataSet.AddProject(project);
            }
            _dataSetRepository.Update(initialDataSet);
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
