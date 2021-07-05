using DataSetExplorer.RepositoryAdapters;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            //IDataSetCreatorService dataSetCreationService = new DataSetCreationService("C:\\main-repository\\", new GitCodeRepository());
            //dataSetCreationService.CreateDataSetSpreadsheet("MyProject", "https://github.com/wieslawsoltes/Core2D/tree/50ea0849bef7e211500764ffcd1fece4a3cb224e");

            //IDataSetAnalyzerService dataSetAnalysisService = new DataSetAnalysisService();
            //dataSetAnalysisService.FindInstancesRequiringAdditionalAnnotation("D:/ccadet/annotations/annotated/ShopifySharp/", "D:/ccadet/annotations/additional/RequireAdditionalAnnotations");
            //dataSetAnalysisService.FindInstancesWithAllDisagreeingAnnotations("D:/ccadet/annotations/annotated/ShopifySharp/", "D:/ccadet/annotations/additional/DisagreeingAnnotations");

            IDataSetExporterService dataSetExportationService = new DataSetExportationService(new FullDataSetFactory());
            dataSetExportationService.Export("D:/ccadet/annotations/annotated/Output/");

            /*IAnnotationConsistencyCheckerService annotationConsistencyService = new AnnotationConsistencyService(new FullDataSetFactory());

            Result<Dictionary<string, Dictionary<string, string>>> metricsSignificanceResult;
            Result<Dictionary<string, string>> annotationConsistencyResult;

            annotationConsistencyResult = annotationConsistencyService.CheckAnnotationConsistencyForAnnotator(1);
            annotationConsistencyResult.Value.ToList().ForEach(result => Console.WriteLine(result.Key + "\n" + result.Value));

            annotationConsistencyResult = annotationConsistencyService.CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(1);
            annotationConsistencyResult.Value.ToList().ForEach(result => Console.WriteLine(result.Key + "\n" + result.Value));

            metricsSignificanceResult = annotationConsistencyService.CheckMetricsSignificanceInAnnotationsForAnnotator(1);
            foreach (var result in metricsSignificanceResult.Value)
            {
                Console.WriteLine(result.Key);
                result.Value.ToList().ForEach(pair => Console.WriteLine(pair.Key + "\n" + pair.Value));
            }

            metricsSignificanceResult = annotationConsistencyService.CheckMetricsSignificanceBetweenAnnotatorsForSeverity(0);
            foreach (var result in metricsSignificanceResult.Value)
            {
                Console.WriteLine(result.Key);
                result.Value.ToList().ForEach(pair => Console.WriteLine(pair.Key + "\n" + pair.Value));
            }*/
        }
    }
}
