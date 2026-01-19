using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using FluentResults;

namespace DataSetExplorer.Core.CleanCodeAnalysis
{
    public interface ICleanCodeAnalysisService
    {
        public Result<string> ExportDatasetAnalysis(int datasetId, CleanCodeAnalysisDTO analysisOptions);
        public Result<string> ExportProjectAnalysis(int projectId, CleanCodeAnalysisDTO analysisOptions);
    }
}
