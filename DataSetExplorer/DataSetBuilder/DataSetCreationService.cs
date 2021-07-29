using DataSetExplorer.DataSetBuilder;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetBuilder.Model.Repository;
using DataSetExplorer.DataSetSerializer;
using DataSetExplorer.DataSetSerializer.ViewModel;
using DataSetExplorer.RepositoryAdapters;
using FluentResults;
using System;
using System.IO;
using System.Threading;
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

        public Result<DataSet> CreateDataSetInDatabase(string basePath, string projectName, string projectAndCommitUrl)
        {
            var initialDataSet = new DataSet(projectAndCommitUrl);
            _dataSetRepository.Create(initialDataSet);
            Task.Run(() => ProcessInitialDataSet(basePath, projectName, projectAndCommitUrl, initialDataSet));
            return Result.Ok(initialDataSet);
        }

        public Result<string> CreateDataSetSpreadsheet(string basePath, string projectName, string projectAndCommitUrl)
        {
            var dataSet = CreateDataSet(basePath, projectName, projectAndCommitUrl);
            return CreateDataSetSpreadsheet(basePath, projectName, new NewSpreadSheetColumnModel(), dataSet);
        }

        public Result<string> CreateDataSetSpreadsheet(string basePath, string projectName, NewSpreadSheetColumnModel columnModel, DataSet dataSet)
        {
            //TODO: Once we establish some DB, we can have the export to excel operation be separate from the "CreateDataSet"
            var excelFileName = ExportToExcel(basePath, projectName, columnModel, dataSet);
            return Result.Ok("Data set exported to " + excelFileName);
        }

        public Result<DataSet> GetDataSet(int id)
        {
            var dataSet = _dataSetRepository.GetDataSet(id);
            if (dataSet == default) return Result.Fail($"DataSet with id: {id} does not exist.");
            return Result.Ok(dataSet);
        }

        private DataSet CreateDataSet(string basePath, string projectName, string projectAndCommitUrl)
        {
            var gitFolderPath = basePath + projectName + Path.DirectorySeparatorChar + "git";
            _codeRepository.SetupRepository(projectAndCommitUrl, gitFolderPath);
            return CreateDataSetFromRepository(projectAndCommitUrl, gitFolderPath);
        }

        private static DataSet CreateDataSetFromRepository(string projectAndCommitUrl, string projectPath)
        {
            //TODO: Introduce Director as a separate class and insert through DI.
            var builder = new CaDETToDataSetBuilder(projectAndCommitUrl, projectPath);
            return builder.IncludeMembersWith(10).IncludeClassesWith(3, 5)
                .RandomizeClassSelection().RandomizeMemberSelection()
                .SetProjectExtractionPercentile(10).Build();
        }

        private void ProcessInitialDataSet(string basePath, string projectName, string projectAndCommitUrl, DataSet initialDataSet)
        {
            var dataSet = CreateDataSet(basePath, projectName, projectAndCommitUrl);
            initialDataSet.AddInstances(dataSet.GetAllInstances());
            initialDataSet.Processed();
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
