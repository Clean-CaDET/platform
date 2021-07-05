using FluentResults;

namespace DataSetExplorer
{
    interface IDataSetAnalyzerService
    {
        public Result<string> FindInstancesWithAllDisagreeingAnnotations(string dataSetPath, string outputPath);
        public Result<string> FindInstancesRequiringAdditionalAnnotation(string dataSetPath, string outputPath);
    }
}
