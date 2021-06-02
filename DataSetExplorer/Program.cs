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
using DataSetExplorer.AnnotationConsistencyTests;
using static DataSetExplorer.AnnotationConsistencyTests.ManovaTest;

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
            //ExportAnnotatedDataSet();
            //CheckAnnotationConsistencyForAnnotator(1);
            //CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(1);
            CheckMetricsSignificanceInAnnotationsForAnnotator(1);
        }

        private static void CheckMetricsSignificanceInAnnotationsForAnnotator(int annotatorId)
        {
            var instancesGroupedBySmells = GetAnnotatedInstancesGroupedBySmells(annotatorId);
            var anovaTest = new AnovaTest();

            foreach (var codeSmellGroup in instancesGroupedBySmells)
            {
                var codeSmell = codeSmellGroup.Key.Replace(" ", "_");
                var metrics = codeSmellGroup.First().MetricFeatures.Keys.ToList();
                var instances = codeSmellGroup.ToList();

                foreach (var metric in metrics)
                {
                    anovaTest.RunTest(annotatorId, instances, codeSmell, metric);
                }
            }
        }

        private static void CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(int severity)
        {
            var instancesGroupedBySmells = GetAnnotatedInstancesGroupedBySmells(annotatorId: null);
            var manovaTest = new ManovaTest();
            manovaTest.Run(instancesGroupedBySmells, severity, new TestDelegate(manovaTest.TestForMultipleAnnotators));
        }

        private static ListDictionary GetAnnotatedProjects()
        {
            ListDictionary projects = new ListDictionary(); // local repository path, annotations folder path
            projects.Add("D:/ccadet/annotations/repos/BurningKnight", "D:/ccadet/annotations/annotated/BurningKnight");
            projects.Add("D:/ccadet/annotations/repos/Core2D", "D:/ccadet/annotations/annotated/Core2d");
            projects.Add("D:/ccadet/annotations/repos/jellyfin", "D:/ccadet/annotations/annotated/Jellyfin");
            projects.Add("D:/ccadet/annotations/repos/OpenRA", "D:/ccadet/annotations/annotated/OpenRA");
            projects.Add("D:/ccadet/annotations/repos/ShareX", "D:/ccadet/annotations/annotated/ShareX");
            projects.Add("D:/ccadet/annotations/repos/ShopifySharp", "D:/ccadet/annotations/annotated/ShopifySharp");
            projects.Add("D:/ccadet/annotations/repos/MonoGame", "D:/ccadet/annotations/annotated/MonoGame");
            projects.Add("D:/ccadet/annotations/repos/Osu", "D:/ccadet/annotations/annotated/Osu");
            return projects;
        }

        private static void CheckAnnotationConsistencyForAnnotator(int annotatorId)
        {
            var instancesGroupedBySmells = GetAnnotatedInstancesGroupedBySmells(annotatorId);
            var manovaTest = new ManovaTest();
            manovaTest.Run(instancesGroupedBySmells, annotatorId, new TestDelegate(manovaTest.TestForSingleAnnotator));
        }

        private static void ExportAnnotatedDataSet()
        {
            var instancesGroupedBySmells = GetAnnotatedInstancesGroupedBySmells(annotatorId: null);
            
            var exporter = new CompleteDataSetExporter("D:/ccadet/annotations/annotated/Output/");
            foreach (var codeSmellGroup in instancesGroupedBySmells)
            {
                exporter.Export(codeSmellGroup.ToList(), "DataSet_" + codeSmellGroup.Key);
            }
        }

        private static IEnumerable<IGrouping<string, DataSetInstance>> GetAnnotatedInstancesGroupedBySmells(int? annotatorId)
        {
            var allAnnotatedInstances = new List<DataSetInstance>();    

            var projects = GetAnnotatedProjects();
            foreach (var key in projects.Keys)
            {
                CodeModelFactory factory = new CodeModelFactory();
                CaDETProject project = factory.CreateProjectWithCodeFileLinks(key.ToString());

                var annotatedInstances = LoadDataSet(projects[key].ToString()).GetAllInstances();
                LoadAnnotators(ref annotatedInstances);
                if (annotatorId != null) annotatedInstances = annotatedInstances.Where(i => i.IsAnnotatedBy((int)annotatorId)).ToList();
                allAnnotatedInstances.AddRange(FillInstancesWithMetrics(annotatedInstances, project));
            }
            return allAnnotatedInstances.GroupBy(i => i.Annotations.ToList()[0].InstanceSmell.Value);
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

        private static List<DataSetInstance> FillInstancesWithMetrics(List<DataSetInstance> annotatedInstances, CaDETProject project)
        {
            return annotatedInstances.Select(i => { 
                i.MetricFeatures = project.GetMetricsForCodeSnippet(i.CodeSnippetId); 
                return i; 
            }).ToList();
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
            var exporter = new NewDataSetExporter("C:/DSOutput/", new ColumnHeuristicsModel(), false);
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
