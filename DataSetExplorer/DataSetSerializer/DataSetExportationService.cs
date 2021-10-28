﻿using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer;
using FluentResults;
using System.Collections.Generic;
using System.IO;

namespace DataSetExplorer
{
    class DataSetExportationService : IDataSetExportationService
    {
        private readonly FullDataSetFactory _fullDataSetFactory;

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
                    exporter.Export(codeSmellGroup.Instances, codeSmellGroup.CodeSmell.Name, "DataSet_" + codeSmellGroup.CodeSmell.Name);
                }
                return Result.Ok("Data set exported: " + outputPath);
            } catch (IOException e)
            {
                return Result.Fail(e.ToString());
            }
        }
    }
}
