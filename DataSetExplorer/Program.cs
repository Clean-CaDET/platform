using DataSetExplorer.DataSetBuilder;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer;
using DataSetExplorer.DataSetSerializer.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using CodeModel;
using CodeModel.CaDETModel;
using CodeModel.CaDETModel.CodeItems;

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

            var dataForExport = PrepareDataForExport(projects);
            var groupedBySmells = dataForExport.GroupBy(t => t.Item1.Annotations.ToList()[0].InstanceSmell.Value);

            var exporter = new DataSetWithMetricsExporter("D:/ccadet/annotations/annotated/Output/");
            var enumerator = groupedBySmells.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var codeSmellGroup = enumerator.Current;
                exporter.Export(codeSmellGroup.ToList(), "DataSet_" + codeSmellGroup.Key);
            }
        }

        private static List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> PrepareDataForExport(ListDictionary projects)
        {
            List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> dataForExport =
                new List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>>();

            foreach (var key in projects.Keys)
            {
                CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);
                CaDETProject project = factory.CreateProjectWithCodeFileLinks(key.ToString());

                var annotatedInstances = LoadDataSet(projects[key].ToString()).GetAllInstances();
                LoadAnnotators(ref annotatedInstances);
                var instanceMetrics = GetMetricsForExport(annotatedInstances, project);
                dataForExport.AddRange(JoinAnnotationsAndMetrics(annotatedInstances, instanceMetrics));
            }
            return dataForExport;
        }

        private static void LoadAnnotators(ref List<DataSetInstance> annotatedInstances)
        {
            List<Annotator> annotators = new List<Annotator>()
            {
                new Annotator(1, 6, 1),
                new Annotator(2, 2, 2),
                new Annotator(3, 2, 3)
            };
            
            foreach (var annotation in annotatedInstances.SelectMany(i => i.Annotations))
            {
                    annotation.Annotator = annotators.Find(annotator => annotator.Id.Equals(annotation.Annotator.Id));
            }
        }

        private static List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>> JoinAnnotationsAndMetrics(List<DataSetInstance> dataSetInstances,
            Dictionary<string, Dictionary<CaDETMetric, double>> datasetInstancesMetrics)
        {
            return dataSetInstances.Select(i => 
                new Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>(i, datasetInstancesMetrics[i.CodeSnippetId]))
                .ToList();
        }

        private static Dictionary<string, Dictionary<CaDETMetric, double>> GetMetricsForExport(List<DataSetInstance> annotatedInstances, CaDETProject project)
        {
            Dictionary<string, Dictionary<CaDETMetric, double>> allMetrics =
                new Dictionary<string, Dictionary<CaDETMetric, double>>();
            foreach (var instance in annotatedInstances)
            {
                allMetrics[instance.CodeSnippetId] = project.GetMetricsForCodeSnippet(instance.CodeSnippetId);
            }
            return allMetrics;
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
            var exporter = new DataSetWithAnnotationsExporter("C:/DSOutput/", new ColumnHeuristicsModel());
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
