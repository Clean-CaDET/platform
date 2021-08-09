using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer;
using FluentResults;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataSetExplorer
{
    class DataSetExportationService : IDataSetExportationService
    {
        private FullDataSetFactory _fullDataSetFactory;

        public DataSetExportationService(FullDataSetFactory fullDataSetFactory)
        {
            _fullDataSetFactory = fullDataSetFactory;
        }

        public Result<string> Export(IDictionary<string, string> projects, List<Annotator> annotators, string outputPath)
        {
            try
            {
                var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(projects, annotators, annotatorId: null);
                var exporter = new CompleteDataSetExporter(outputPath);
                foreach (var codeSmellGroup in instancesGroupedBySmells)
                {
                    exporter.Export(codeSmellGroup.ToList(), codeSmellGroup.Key, "DataSet_" + codeSmellGroup.Key);
                }
                return Result.Ok("Data set exported: " + outputPath);
            } catch (IOException e)
            {
                return Result.Fail(e.ToString());
            }
        }
    }
}
