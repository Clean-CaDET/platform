using DataSetExplorer.DataSetBuilder;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer;
using System.Collections.Generic;
using DataSetExplorer.DataSetSerializer.ViewModel;

namespace DataSetExplorer
{
    class Program
    {
        /*
         *https://github.com/jellyfin/jellyfin/tree/6c2eb5fc7e872a29b4a0951849681ae0764dbb8e
9.5k Zvezda I 20k commitova

https://github.com/MonoGame/MonoGame/tree/4802d00db04dc7aa5fe07cd2d908f9a4b090a4fd
7.3k Zvezda I 13k commitova

https://github.com/ppy/osu/tree/2cac373365309a40474943f55c56159ed8f9433c
6.2k Zvezda, 37k commitova

https://github.com/ThreeMammals/Ocelot/tree/3ef6abd7465fc77632e4b2d5189fbbf47b457867
6k Zvezda, 1.2k commita

https://github.com/dotnet/machinelearning/tree/44660297b4238a4f3e843bd071f5e8b214bdd38a

         *
         */
        static void Main(string[] args)
        {
            //MakeExcelFromProjectUseCase();
            FindInstancesRequiringAdditionalAnnotationUseCase();
        }

        private static void FindInstancesRequiringAdditionalAnnotationUseCase()
        {
            var importer = new ExcelImporter("C:/DSInput");
            var dataset = importer.Import("Clean CaDET");

            var exporter = new TextFileExporter("C:/DSOutput/");
            exporter.ExportInstancesWithAnnotatorId(dataset.GetInsufficientlyAnnotatedInstances());
        }

        private static void MakeExcelFromProjectUseCase()
        {
            var dataSet = CreateDataSetFromRepository(
                "https://github.com/jellyfin/jellyfin/tree/6c2eb5fc7e872a29b4a0951849681ae0764dbb8e",
                "C:/sdataset-p2/jellyfin");
            var exporter = new ExcelExporter("C:/DSOutput/", new ColumnHeuristicsModel());
            exporter.Export(dataSet, "jellyfin");
        }

        private static DataSet CreateDataSetFromRepository(string projectAndCommitUrl, string projectPath)
        {
            var builder = new CaDETToDataSetBuilder(projectAndCommitUrl, projectPath);
            return builder.IncludeMembersWith(3).IncludeClassesWith(2, 4).RandomizeClassSelection().RandomizeMemberSelection()
                .SetProjectExtractionPercentile(10).Build();
        }
    }
}
