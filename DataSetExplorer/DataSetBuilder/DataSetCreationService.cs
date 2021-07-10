using DataSetExplorer.DataSetBuilder;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer;
using DataSetExplorer.DataSetSerializer.ViewModel;
using DataSetExplorer.RepositoryAdapters;
using FluentResults;
using System;
using System.IO;

namespace DataSetExplorer
{
    public class DataSetCreationService : IDataSetCreationService
    {
        private readonly ICodeRepository _codeRepository;

        public DataSetCreationService(ICodeRepository codeRepository)
        {
            _codeRepository = codeRepository;
        }

        public Result<string> CreateDataSetSpreadsheet(string basePath, string projectName, string projectAndCommitUrl)
        {
            return CreateDataSetSpreadsheet(basePath, projectName, projectAndCommitUrl, new NewSpreadSheetColumnModel());
        }

        public Result<string> CreateDataSetSpreadsheet(string basePath, string projectName, string projectAndCommitUrl, NewSpreadSheetColumnModel columnModel)
        {
            //TODO: Once we establish some DB, we can have the export to excel operation be separate from the "CreateDataSet"
            var gitFolderPath = basePath + projectName + Path.DirectorySeparatorChar + "git";
            _codeRepository.SetupRepository(projectAndCommitUrl, gitFolderPath);
            
            var dataSet = CreateDataSetFromRepository(projectAndCommitUrl, gitFolderPath);
            var excelFileName = ExportToExcel(basePath, projectName, columnModel, dataSet);
            
            return Result.Ok("Data set created: " + excelFileName);
        }

        private static DataSet CreateDataSetFromRepository(string projectAndCommitUrl, string projectPath)
        {
            //TODO: Introduce Director as a separate class and insert through DI.
            var builder = new CaDETToDataSetBuilder(projectAndCommitUrl, projectPath);
            return builder.IncludeMembersWith(10).IncludeClassesWith(3, 5)
                .RandomizeClassSelection().RandomizeMemberSelection()
                .SetProjectExtractionPercentile(10).Build();
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
