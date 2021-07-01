using DataSetExplorer.DataSetSerializer;
using System.Linq;

namespace DataSetExplorer
{
    class DataSetExportationService : IDataSetExporter
    {
        private IFullDataSetBuilder _fullDataSetBuilder;

        public DataSetExportationService(IFullDataSetBuilder fullDataSetBuilder)
        {
            _fullDataSetBuilder = fullDataSetBuilder;
        }

        public void Export(string outputPath)
        {
            var instancesGroupedBySmells = _fullDataSetBuilder.GetAnnotatedInstancesGroupedBySmells(annotatorId: null);
            var exporter = new CompleteDataSetExporter(outputPath);
            foreach (var codeSmellGroup in instancesGroupedBySmells)
            {
                exporter.Export(codeSmellGroup.ToList(), codeSmellGroup.Key, "DataSet_" + codeSmellGroup.Key);
            }
        }
    }
}
