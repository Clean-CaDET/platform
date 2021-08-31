using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetBuilder.Model.Repository;
using DataSetExplorer.DataSetSerializer;
using FluentResults;
using System.Collections.Generic;
using System.IO;

namespace DataSetExplorer
{
    public class DataSetAnalysisService : IDataSetAnalysisService
    {
        private readonly IDataSetRepository _dataSetRepository;
        private readonly IDataSetProjectRepository _dataSetProjectRepository;
        public DataSetAnalysisService(IDataSetRepository dataSetRepository, IDataSetProjectRepository dataSetProjectRepository)
        {
            _dataSetRepository = dataSetRepository;
            _dataSetProjectRepository = dataSetProjectRepository;
        }

        public Result<string> FindInstancesWithAllDisagreeingAnnotations(IDictionary<string, string> projects)
        {
            try
            {
                foreach (var projectFolderPath in projects.Keys)
                {
                    var project = LoadDataSetProject(projectFolderPath, projectFolderPath);
                    var exporter = new TextFileExporter(projects[projectFolderPath]);
                    exporter.ExportInstancesWithAnnotatorId(project.GetInstancesWithAllDisagreeingAnnotations());
                }
                return Result.Ok("Instances with disagreeing annotations exported.");
            } catch (IOException e)
            {
                return Result.Fail(e.ToString());
            }
        }

        public Result<string> FindInstancesRequiringAdditionalAnnotation(IDictionary<string, string> projects)
        {
            try
            {
                foreach (var projectFolderPath in projects.Keys)
                {
                    var project = LoadDataSetProject(projectFolderPath, projectFolderPath);
                    var exporter = new TextFileExporter(projects[projectFolderPath]);
                    exporter.ExportInstancesWithAnnotatorId(project.GetInsufficientlyAnnotatedInstances());
                }
                return Result.Ok("Instances requiring additional annotation exported.");
            } catch (IOException e)
            {
                return Result.Fail(e.ToString());
            }
        }

        public Result<List<DataSetInstance>> FindInstancesWithAllDisagreeingAnnotations(IEnumerable<int> projectIds)
        {
            var instances = new List<DataSetInstance>();
            var projects = _dataSetProjectRepository.GetDataSetProjects(projectIds);
            foreach (var project in projects) instances.AddRange(project.GetInstancesWithAllDisagreeingAnnotations());

            return Result.Ok(instances);
        }

        public Result<List<DataSetInstance>> FindInstancesRequiringAdditionalAnnotation(IEnumerable<int> projectIds)
        {
            var instances = new List<DataSetInstance>();
            var projects = _dataSetProjectRepository.GetDataSetProjects(projectIds);
            foreach (var project in projects) instances.AddRange(project.GetInsufficientlyAnnotatedInstances());

            return Result.Ok(instances);
        }

        private DataSetProject LoadDataSetProject(string folder, string projectName)
        {
            var importer = new ExcelImporter(folder);
            return importer.Import(projectName);
        }
    }
}
