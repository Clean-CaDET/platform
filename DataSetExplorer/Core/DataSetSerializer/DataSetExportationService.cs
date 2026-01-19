using System.Collections.Generic;
using System.IO;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.Auth;
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
        private readonly ICompleteDataSetExportationService _completeDataSetExportationService;
        private readonly IDataSetRepository _dataSetRepository;
        private readonly IAuthService _authService;

        public DataSetExportationService(FullDataSetFactory fullDataSetFactory, IDraftDataSetExportationService draftDataSetExportationService, 
            IDataSetRepository dataSetRepository, ICompleteDataSetExportationService completeDataSetExportationService,
            IAuthService authService)
        {
            _fullDataSetFactory = fullDataSetFactory;
            _dataSetRepository = dataSetRepository;
            _draftDataSetExportationService = draftDataSetExportationService;
            _completeDataSetExportationService = completeDataSetExportationService;
            _authService = authService;
        }

        public Result<string> ExportDraft(DraftDataSetExportDTO dataSetDTO)
        {
            var dataSet = GetDataSetForExport(dataSetDTO.Id).Value;
            var exportPath = _draftDataSetExportationService.Export(dataSetDTO.ExportPath, dataSetDTO.AnnotatorId, dataSet);
            return Result.Ok(exportPath);
        }

        public Result<string> ExportComplete(int datasetId, CompleteDataSetExportDTO dataSetDTO)
        { 
            var annotationsFilesPaths = File.ReadAllLines(dataSetDTO.AnnotationsPath);
            return this.Export(datasetId, annotationsFilesPaths, dataSetDTO.ExportPath);
        }

        private Result<DataSet> GetDataSetForExport(int id)
        {
            var dataSet = _dataSetRepository.GetDataSetForExport(id);
            if (dataSet == default) return Result.Fail($"DataSet with id: {id} does not exist.");
            return Result.Ok(dataSet);
        }

        public Result<string> Export(int datasetId, string[] annotationsFilesPaths, string outputPath)
        {
            List<Annotator> annotators = _authService.GetAll().Value;
            try
            {
                var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(datasetId, annotators, annotationsFilesPaths);
                foreach (var codeSmellGroup in instancesGroupedBySmells)
                {
                    _completeDataSetExportationService.Export(outputPath, codeSmellGroup.Instances, codeSmellGroup.CodeSmell.Name, "DataSet_" + codeSmellGroup.CodeSmell.Name);
                }
                return Result.Ok("Data set exported: " + outputPath);
            } catch (IOException e)
            {
                return Result.Fail(e.ToString());
            }
        }
    }
}
