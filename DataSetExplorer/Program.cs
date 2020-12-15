using DataSetExplorer.DataSetBuilder;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetImporter;

namespace DataSetExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateDataSetFromRepository();
        }

        private static DataSet ReadDataSetFromExcel()
        {
            var importer = new ExcelImporter("C:\\DSInput\\");
            return importer.Import("BurningKnight");
        }

        private static void CreateDataSetFromRepository()
        {
            var builder = new CaDETToDataSetBuilder("https://github.com/OpenRA/OpenRA/tree/920d00bbae9fa8e62387bbff705ca4bea6a26677", "C:\\sdataset5\\");

            var project = builder.IncludeMembersWith(2).RandomizeClassSelection().RandomizeMemberSelection()
                .SetProjectExtractionPercentile(10).Build();

            var fileSerializer = new TextFileExporter("C:\\DSOutput\\", project);
            fileSerializer.ExtractNamesToFile();
        }
    }
}
