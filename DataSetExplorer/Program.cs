using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using CodeModel;
using CodeModel.CaDETModel;
using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.AnnotationConsistencyTests;
using DataSetExplorer.RepositoryAdapters;

namespace DataSetExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            IDataSetCreator service = new DataSetCreationService("C:\\main-repository\\", new GitCodeRepository());
            service.CreateDataSetSpreadsheet("MyProject",
                "https://github.com/wieslawsoltes/Core2D/tree/50ea0849bef7e211500764ffcd1fece4a3cb224e");
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
            IMetricsSignificanceTester tester = new AnovaTest();
            var results = tester.Test(annotatorId, instancesGroupedBySmells);
            foreach (var result in results.Value)
            {
                Console.WriteLine(result.Key);
                result.Value.ToList().ForEach(pair => Console.WriteLine(pair.Key + "\n" + pair.Value));
            }
        }

        private static void CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(int severity)
        {
            var instancesGroupedBySmells = GetAnnotatedInstancesGroupedBySmells(annotatorId: null);
            IAnnotatorsConsistencyTester tester = new ManovaTest();
            var results = tester.TestConsistencyBetweenAnnotators(severity, instancesGroupedBySmells);
            results.Value.ToList().ForEach(result => Console.WriteLine(result.Key + "\n" + result.Value));
        }

        private static void CheckAnnotationConsistencyForAnnotator(int annotatorId)
        {
            var instancesGroupedBySmells = GetAnnotatedInstancesGroupedBySmells(annotatorId);
            IAnnotatorsConsistencyTester tester = new ManovaTest();
            var results = tester.TestConsistencyOfSingleAnnotator(annotatorId, instancesGroupedBySmells);
            results.Value.ToList().ForEach(result => Console.Write(result.Key + "\n" + result.Value));
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
    }
}