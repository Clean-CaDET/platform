using DataSetExplorer.DataSetBuilder;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetImporter;

namespace DataSetExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            //CreateDataSetFromRepository();
            DataSet ds = ReadDataSetFromExcel();
            //TODO: Srediti excele da imaju anotator ID, uparene heuristike, i sve ostalo sto treba
            var b = ds.Name;
        }

        private static DataSet ReadDataSetFromExcel()
        {
            var importer = new ExcelImporter("C:\\DSInput\\");
            return importer.Import("BurningKnight");
        }

        private static void CreateDataSetFromRepository()
        {
            var builder = new CaDETToDataSetBuilder("SharpX", "C:\\sdataset4\\");

            var project = builder.IncludeMembersWith(2).RandomizeClassSelection().RandomizeMemberSelection()
                .SetProjectExtractionPercentile(10).Build();

            var fileSerializer = new TextFileExporter("C:\\DSOutput\\", project);
            fileSerializer.ExtractNamesToFile();
        }
    }
}
