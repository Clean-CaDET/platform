using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer;
using FluentResults;
using System;

namespace DataSetExplorer
{
    class DataSetAnalysisService : IDataSetAnalyzerService
    {
        public Result<string> FindInstancesWithAllDisagreeingAnnotations(string dataSetPath, string outputPath)
        {
            try
            {
                var dataset = LoadDataSet(dataSetPath);
                var exporter = new TextFileExporter(outputPath);
                exporter.ExportInstancesWithAnnotatorId(dataset.GetInstancesWithAllDisagreeingAnnotations());
                return Result.Ok("Instances with disagreeing annotations exported: " + outputPath);
            } catch (Exception e)
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
            } catch (Exception e)
            {
                return Result.Fail(e.ToString());
            }
        }

        private DataSet LoadDataSet(string folder)
        {
            var importer = new ExcelImporter(folder);
            return importer.Import("Clean CaDET");
        }
    }
}
