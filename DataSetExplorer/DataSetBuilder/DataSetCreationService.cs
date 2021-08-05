using DataSetExplorer.DataSetBuilder;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer;
using DataSetExplorer.DataSetSerializer.ViewModel;
using DataSetExplorer.RepositoryAdapters;
using FluentResults;
using System;
using System.Collections.Specialized;
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

        public Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, ListDictionary projects)
        {
            return CreateDataSetSpreadsheet(dataSetName, basePath, projects, new NewSpreadSheetColumnModel());
        }

        public Result<string> CreateDataSetSpreadsheet(string dataSetName, string basePath, ListDictionary projects, NewSpreadSheetColumnModel columnModel)
        {
            //TODO: Once we establish some DB, we can have the export to excel operation be separate from the "CreateDataSet"
            var dataSet = new DataSet(dataSetName);
            foreach(var projectName in projects.Keys)
            {
                var gitFolderPath = basePath + projectName.ToString() + Path.DirectorySeparatorChar + "git";
                _codeRepository.SetupRepository(projects[projectName].ToString(), gitFolderPath);
                var dataSetProject = CreateDataSetProjectFromRepository(projects[projectName].ToString(), projectName.ToString(), gitFolderPath);
                dataSet.AddProject(dataSetProject);
            }

            var excelFileName = ExportToExcel(basePath, dataSetName, columnModel, dataSet);
            return Result.Ok("Data set created: " + excelFileName);
        }

        private static DataSetProject CreateDataSetProjectFromRepository(string projectAndCommitUrl, string projectName, string projectPath)
        {
            //TODO: Introduce Director as a separate class and insert through DI.
            var builder = new CaDETToDataSetProjectBuilder(projectAndCommitUrl, projectName, projectPath);
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
