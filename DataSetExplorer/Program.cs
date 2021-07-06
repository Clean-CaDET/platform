using DataSetExplorer.RepositoryAdapters;

namespace DataSetExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            //IDataSetCreationService dataSetCreationService = new DataSetCreationService("C:\\main-repository\\", new GitCodeRepository());
            //dataSetCreationService.CreateDataSetSpreadsheet("MyProject", "https://github.com/wieslawsoltes/Core2D/tree/50ea0849bef7e211500764ffcd1fece4a3cb224e");

            //IDataSetAnalysisService dataSetAnalysisService = new DataSetAnalysisService();
            //dataSetAnalysisService.FindInstancesRequiringAdditionalAnnotation("D:/ccadet/annotations/annotated/ShopifySharp/", "D:/ccadet/annotations/additional/RequireAdditionalAnnotations");
            //dataSetAnalysisService.FindInstancesWithAllDisagreeingAnnotations("D:/ccadet/annotations/annotated/ShopifySharp/", "D:/ccadet/annotations/additional/DisagreeingAnnotations");

            IDataSetExportationService dataSetExportationService = new DataSetExportationService(new FullDataSetFactory());
            dataSetExportationService.Export("D:/ccadet/annotations/annotated/Output/");

            /*IAnnotationConsistencyService annotationConsistencyService = new AnnotationConsistencyService(new FullDataSetFactory());
            annotationConsistencyService.CheckAnnotationConsistencyForAnnotator(1);
            annotationConsistencyService.CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(1);
            annotationConsistencyService.CheckMetricsSignificanceInAnnotationsForAnnotator(1);
            annotationConsistencyService.CheckMetricsSignificanceBetweenAnnotatorsForSeverity(0);*/
        }
    }
}
