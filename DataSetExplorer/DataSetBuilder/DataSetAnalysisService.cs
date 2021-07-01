using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer;

namespace DataSetExplorer
{
    class DataSetAnalysisService : IDataSetAnalyzer
    {
        public void FindInstancesWithAllDisagreeingAnnotationsUseCase(string dataSetPath, string outputPath)
        {
            var dataset = LoadDataSet(dataSetPath);
            var exporter = new TextFileExporter(outputPath);
            exporter.ExportInstancesWithAnnotatorId(dataset.GetInstancesWithAllDisagreeingAnnotations());
        }

        public void FindInstancesRequiringAdditionalAnnotationUseCase(string dataSetPath, string outputPath)
        {
            var dataset = LoadDataSet(dataSetPath);
            var exporter = new TextFileExporter(outputPath);
            exporter.ExportInstancesWithAnnotatorId(dataset.GetInsufficientlyAnnotatedInstances());
        }

        private DataSet LoadDataSet(string folder)
        {
            var importer = new ExcelImporter(folder);
            return importer.Import("Clean CaDET");
        }
    }
}
