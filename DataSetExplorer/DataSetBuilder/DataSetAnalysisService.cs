using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer;
using FluentResults;
using System.Collections.Specialized;
using System.IO;

namespace DataSetExplorer
{
    class DataSetAnalysisService : IDataSetAnalysisService
    {
        public Result<string> FindInstancesWithAllDisagreeingAnnotations(ListDictionary projects)
        {
            try
            {
                foreach (var projectFolderPath in projects.Keys)
                {
                    var project = LoadDataSetProject(projectFolderPath.ToString(), projectFolderPath.ToString());
                    var exporter = new TextFileExporter(projects[projectFolderPath].ToString());
                    exporter.ExportInstancesWithAnnotatorId(project.GetInstancesWithAllDisagreeingAnnotations());
                }
                return Result.Ok("Instances with disagreeing annotations exported.");
            } catch (IOException e)
            {
                return Result.Fail(e.ToString());
            }
        }

        public Result<string> FindInstancesRequiringAdditionalAnnotation(ListDictionary projects)
        {
            try
            {
                foreach (var projectFolderPath in projects.Keys)
                {
                    var project = LoadDataSetProject(projectFolderPath.ToString(), projectFolderPath.ToString());
                    var exporter = new TextFileExporter(projects[projectFolderPath].ToString());
                    exporter.ExportInstancesWithAnnotatorId(project.GetInsufficientlyAnnotatedInstances());
                }
                return Result.Ok("Instances requiring additional annotation exported.");
            } catch (IOException e)
            {
                return Result.Fail(e.ToString());
            }
        }

        private DataSetProject LoadDataSetProject(string folder, string projectName)
        {
            var importer = new ExcelImporter(folder);
            return importer.Import(projectName);
        }
    }
}
