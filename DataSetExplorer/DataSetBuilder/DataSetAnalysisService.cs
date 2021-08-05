using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetBuilder.Model.Repository;
using DataSetExplorer.DataSetSerializer;
using FluentResults;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace DataSetExplorer
{
    public class DataSetAnalysisService : IDataSetAnalysisService
    {
        private readonly IDataSetRepository _dataSetRepository;
        public DataSetAnalysisService(IDataSetRepository dataSetRepository)
        {
            _dataSetRepository = dataSetRepository;
        }

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

        public Result<List<DataSetInstance>> FindInstancesWithAllDisagreeingAnnotations(int dataSetId)
        {
            var dataset = _dataSetRepository.GetDataSet(dataSetId);
            if (dataset == default) return Result.Fail($"DataSet with id: {dataSetId} does not exist.");

            var instances = new List<DataSetInstance>();
            foreach (var project in dataset._projects)
            {
                instances.AddRange(project.GetInstancesWithAllDisagreeingAnnotations());
            }
            return Result.Ok(instances);
        }

        public Result<List<DataSetInstance>> FindInstancesRequiringAdditionalAnnotation(int dataSetId)
        {
            var dataset = _dataSetRepository.GetDataSet(dataSetId);
            if (dataset == default) return Result.Fail($"DataSet with id: {dataSetId} does not exist.");

            var instances = new List<DataSetInstance>();
            foreach (var project in dataset._projects)
            {
                instances.AddRange(project.GetInsufficientlyAnnotatedInstances());
            }
            return Result.Ok(instances);
        }

        private DataSetProject LoadDataSetProject(string folder, string projectName)
        {
            var importer = new ExcelImporter(folder);
            return importer.Import(projectName);
        }
    }
}
