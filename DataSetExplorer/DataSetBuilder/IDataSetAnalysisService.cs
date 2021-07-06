using FluentResults;

namespace DataSetExplorer
{
    interface IDataSetAnalysisService
    {
        public Result<string> FindInstancesWithAllDisagreeingAnnotations(string dataSetPath, string outputPath);
        public Result<string> FindInstancesRequiringAdditionalAnnotation(string dataSetPath, string outputPath);
    }
}
