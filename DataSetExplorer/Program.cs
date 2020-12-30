using DataSetExplorer.DataSetBuilder;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer;
using DataSetExplorer.DataSetSerializer.ViewModel;

namespace DataSetExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            var ds = CreateDataSetFromRepository();
            var exporter = new ExcelExporter("C:\\DSOutput\\", new ColumnHeuristicsModel());
            exporter.Export(ds, "ShopifySharp");
        }

        private static DataSet ReadDataSetFromExcel()
        {
            var importer = new ExcelImporter("C:\\DSInput\\");
            return importer.Import("BurningKnight");
        }

        private static DataSet CreateDataSetFromRepository()
        {
            var builder = new CaDETToDataSetBuilder("https://github.com/OpenRA/OpenRA/tree/920d00bbae9fa8e62387bbff705ca4bea6a26677", "C:\\sdataset2\\");
            return builder.IncludeMembersWith(3).IncludeClassesWith(2, 4).RandomizeClassSelection().RandomizeMemberSelection()
                .SetProjectExtractionPercentile(10).Build();
        }
    }
}
