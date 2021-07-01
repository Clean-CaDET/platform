using DataSetExplorer.RepositoryAdapters;

namespace DataSetExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            //IDataSetCreator dataSetCreationService = new DataSetCreationService("C:\\main-repository\\", new GitCodeRepository());
            //dataSetCreationService.CreateDataSetSpreadsheet("MyProject", "https://github.com/wieslawsoltes/Core2D/tree/50ea0849bef7e211500764ffcd1fece4a3cb224e");

            //IDataSetAnalyzer dataSetAnalysisService = new DataSetAnalysisService();
            //dataSetAnalysisService.FindInstancesRequiringAdditionalAnnotationUseCase("D:/ccadet/annotations/annotated/ShopifySharp/", "D:/ccadet/annotations/additional/RequireAdditionalAnnotations");
            //dataSetAnalysisService.FindInstancesWithAllDisagreeingAnnotationsUseCase("D:/ccadet/annotations/annotated/ShopifySharp/", "D:/ccadet/annotations/additional/DisagreeingAnnotations");

            IDataSetExporter dataSetExportationService = new DataSetExportationService(new FullDataSetBuilderService());
            dataSetExportationService.Export("D:/ccadet/annotations/annotated/Output/");

            /*IAnnotationConsistencyChecker annotationConsistencyService = new AnnotationConsistencyService(new FullDataSetBuilderService());
            annotationConsistencyService.CheckAnnotationConsistencyForAnnotator(1);
            annotationConsistencyService.CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(1);
            annotationConsistencyService.CheckMetricsSignificanceInAnnotationsForAnnotator(1);
            annotationConsistencyService.CheckMetricsSignificanceBetweenAnnotatorsForSeverity(0);*/
        }
    }
}
