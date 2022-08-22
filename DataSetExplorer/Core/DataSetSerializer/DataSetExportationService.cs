using System.Collections.Generic;
using System.IO;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.Core.DataSets.Repository;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using FluentResults;

namespace DataSetExplorer.Core.DataSetSerializer
{
    class DataSetExportationService : IDataSetExportationService
    {
        private readonly FullDataSetFactory _fullDataSetFactory;
        private readonly IDraftDataSetExportationService _draftDataSetExportationService;
        private readonly IDataSetRepository _dataSetRepository;

        public DataSetExportationService(FullDataSetFactory fullDataSetFactory, IDraftDataSetExportationService draftDataSetExportationService, 
            IDataSetRepository dataSetRepository)
        {
            _fullDataSetFactory = fullDataSetFactory;
            _dataSetRepository = dataSetRepository;
            _draftDataSetExportationService = draftDataSetExportationService;
        }

        public Result<string> ExportDraft(DraftDataSetExportDTO dataSetDTO)
        {
            var dataSet = GetDataSetForExport(dataSetDTO.Id).Value;
            var exportPath = _draftDataSetExportationService.Export(dataSetDTO.ExportPath, dataSetDTO.AnnotatorId, dataSet);
            return Result.Ok(exportPath);
        }

        private Result<DataSet> GetDataSetForExport(int id)
        {
            var dataSet = _dataSetRepository.GetDataSetForExport(id);
            if (dataSet == default) return Result.Fail($"DataSet with id: {id} does not exist.");
            return Result.Ok(dataSet);
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
