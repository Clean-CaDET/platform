using DataSetExplorer.DataSetSerializer;
using FluentResults;
using System;
using System.Linq;

namespace DataSetExplorer
{
    class DataSetExportationService : IDataSetExporterService
    {
        private FullDataSetFactory _fullDataSetFactory;

        public DataSetExportationService(FullDataSetFactory fullDataSetFactory)
        {
            _fullDataSetFactory = fullDataSetFactory;
        }

        public Result<string> Export(string outputPath)
        {
            try
            {
                var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(annotatorId: null);
                var exporter = new CompleteDataSetExporter(outputPath);
                foreach (var codeSmellGroup in instancesGroupedBySmells)
                {
                    exporter.Export(codeSmellGroup.ToList(), codeSmellGroup.Key, "DataSet_" + codeSmellGroup.Key);
                }
                return Result.Ok("Data set exported: " + outputPath);
            } catch (Exception e)
            {
                return Result.Fail(e.ToString());
            }
        }
    }
}
