using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetBuilder.Model.Repository;
using DataSetExplorer.DataSetSerializer;
using FluentResults;
using System;
using System.Collections.Generic;
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

        public Result<string> FindInstancesWithAllDisagreeingAnnotations(string dataSetPath, string outputPath)
        {
            try
            {
                var dataset = LoadDataSet(dataSetPath);
                var exporter = new TextFileExporter(outputPath);
                exporter.ExportInstancesWithAnnotatorId(dataset.GetInstancesWithAllDisagreeingAnnotations());
                return Result.Ok("Instances with disagreeing annotations exported: " + outputPath);
            }
            catch (IOException e)
            {
                return Result.Fail(e.ToString());
            }
        }

        public Result<string> FindInstancesRequiringAdditionalAnnotation(string dataSetPath, string outputPath)
        {
            try
            {
                var dataset = LoadDataSet(dataSetPath);
                var exporter = new TextFileExporter(outputPath);
                exporter.ExportInstancesWithAnnotatorId(dataset.GetInsufficientlyAnnotatedInstances());
                return Result.Ok("Instances requiring additional annotation exported: " + outputPath);
            }
            catch (IOException e)
            {
                return Result.Fail(e.ToString());
            }
        }

        public Result<List<DataSetInstance>> FindInstancesWithAllDisagreeingAnnotations(int dataSetId)
        {
            var dataset = _dataSetRepository.GetDataSet(dataSetId);
            if (dataset == default) return Result.Fail($"DataSet with id: {dataSetId} does not exist.");
            var instances = dataset.GetInstancesWithAllDisagreeingAnnotations();
            return Result.Ok(instances);
        }

        public Result<List<DataSetInstance>> FindInstancesRequiringAdditionalAnnotation(int dataSetId)
        {
            var dataset = _dataSetRepository.GetDataSet(dataSetId);
            if (dataset == default) return Result.Fail($"DataSet with id: {dataSetId} does not exist.");
            var instances = dataset.GetInsufficientlyAnnotatedInstances();
            return Result.Ok(instances);
        }

        private DataSet LoadDataSet(string folder)
        {
            var importer = new ExcelImporter(folder);
            return importer.Import("Clean CaDET");
        }
    }
}
