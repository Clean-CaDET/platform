using DataSetExplorer.DataSetBuilder;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer;
using DataSetExplorer.DataSetSerializer.ViewModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using RepositoryCompiler.CodeModel;
using RepositoryCompiler.CodeModel.CaDETModel;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;

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
            //FindInstancesRequiringAdditionalAnnotationUseCase();
            //FindInstancesWithAllDisagreeingAnnotationsUseCase();
            ExportAnnotatedDataSet();
        }
        
        private static void ExportAnnotatedDataSet()
        {
            ListDictionary projects = new ListDictionary(); // local repository path, annotations folder path
            projects.Add("D:/ccadet/annotations/repos/BurningKnight", "D:/ccadet/annotations/annotated/BurningKnight");
            projects.Add("D:/ccadet/annotations/repos/Core2D", "D:/ccadet/annotations/annotated/Core2d");
            projects.Add("D:/ccadet/annotations/repos/jellyfin", "D:/ccadet/annotations/annotated/Jellyfin");
            projects.Add("D:/ccadet/annotations/repos/OpenRA", "D:/ccadet/annotations/annotated/OpenRA");
            projects.Add("D:/ccadet/annotations/repos/ShareX", "D:/ccadet/annotations/annotated/ShareX");
            projects.Add("D:/ccadet/annotations/repos/ShopifySharp", "D:/ccadet/annotations/annotated/ShopifySharp");

            var dataForExport = new List<ExportedDataSetInstance>();
            foreach (var key in projects.Keys)
            {
                CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);
                CaDETProject project = factory.CreateProjectWithCodeFileLinks(key.ToString());
                
                var dataset = LoadDataSet(projects[key].ToString());
                var annotatedInstances = dataset.GetAllInstances();
                dataForExport.AddRange(GetDataForExport(annotatedInstances, project));
            }
            
            var exporter = new DataSetExporter("C:/dataset/output/");
            exporter.Export(dataForExport, "DataSet");
        }

        private static List<ExportedDataSetInstance> GetDataForExport(List<DataSetInstance> annotatedInstances, CaDETProject project)
        {
            List<ExportedDataSetInstance> dataForExport = new List<ExportedDataSetInstance>();
            foreach (var instance in annotatedInstances)
            {
                var exportedDataSetInstance = new ExportedDataSetInstance(
                    instance.CodeSnippetId,
                    instance.Link,
                    instance.Annotations.ToList()[0].InstanceSmell.Value,
                    instance.ProjectLink)
                {
                    Metrics = GetMetrics(instance, project),
                    Annotations = instance.Annotations.ToList()
                };
                dataForExport.Add(exportedDataSetInstance);
            }
            return dataForExport;
        }

        private static Dictionary<CaDETMetric, double> GetMetrics(DataSetInstance instance, CaDETProject project)
        {
            if (instance.Type == SnippetType.Class)
            {
                CaDETClass classInstance = project.Classes.First(c => c.FullName.Equals(instance.CodeSnippetId));
                return classInstance.Metrics;
            }

            CaDETMember memberInstance = null;
            foreach (var cl in project.Classes)
            {
                memberInstance = cl.Members.FirstOrDefault(m => m.ToString().Equals(instance.CodeSnippetId));
                if (memberInstance != null) break;
            }
            return memberInstance?.Metrics;
        }

        private static void FindInstancesWithAllDisagreeingAnnotationsUseCase()
        {
            var dataset = LoadDataSet("C:/DSInput");

            var exporter = new TextFileExporter("C:/DSOutput/Mono");
            exporter.ExportInstancesWithAnnotatorId(dataset.GetInstancesWithAllDisagreeingAnnotations());
        }

        private static void FindInstancesRequiringAdditionalAnnotationUseCase()
        {
            var dataset = LoadDataSet("C:/DSInput/MonoGame");

            var exporter = new TextFileExporter("C:/DSOutput/Mono");
            exporter.ExportInstancesWithAnnotatorId(dataset.GetInsufficientlyAnnotatedInstances());
        }

        private static DataSet LoadDataSet(string folder)
        {
            var importer = new ExcelImporter(folder);
            return importer.Import("Clean CaDET");
        }

        private static void MakeExcelFromProjectUseCase()
        {
            var dataSet = CreateDataSetFromRepository(
                "https://github.com/MonoGame/MonoGame/tree/4802d00db04dc7aa5fe07cd2d908f9a4b090a4fd",
                "C:/sdataset-p2/MonoGame");
            var exporter = new ExcelExporter("C:/DSOutput/", new ColumnHeuristicsModel());
            exporter.Export(dataSet, "MonoGame");
        }

        private static DataSet CreateDataSetFromRepository(string projectAndCommitUrl, string projectPath)
        {
            var builder = new CaDETToDataSetBuilder(projectAndCommitUrl, projectPath);
            return builder.IncludeMembersWith(10).IncludeClassesWith(3, 5).RandomizeClassSelection().RandomizeMemberSelection()
              .SetProjectExtractionPercentile(10).Build();
        }
    }
}
