using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using FluentResults;

namespace DataSetExplorer.Core.DataSetSerializer
{
    public interface IDataSetExportationService
    {
        public Result<string> Export(int datasetId, string[] annotationsFilesPaths, string outputPath);
        public Result<string> ExportDraft(DraftDataSetExportDTO dataSetDTO);
        public Result<string> ExportComplete(int datasetId, CompleteDataSetExportDTO dataSetDTO);
    }
}
