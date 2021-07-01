namespace DataSetExplorer
{
    interface IDataSetAnalyzer
    {
        public void FindInstancesWithAllDisagreeingAnnotationsUseCase(string dataSetPath, string outputPath);
        public void FindInstancesRequiringAdditionalAnnotationUseCase(string dataSetPath, string outputPath);
    }
}
