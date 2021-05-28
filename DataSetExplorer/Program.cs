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
        }

        private static void CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(int severity)
        {
            var dataGroupedBySmells = GetAnnotatedInstancesGroupedBySmells(annotatorId: null);

            var exporter = new AnnotationConsistencyByMetricsExporter("D:/ccadet/annotations/sanity_check/Output/");
            var manovaTest = new ManovaTest.ManovaTest();

            var enumerator = dataGroupedBySmells.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var codeSmellGroup = enumerator.Current;
                var codeSmell = codeSmellGroup.Key.Replace(" ", "_");

                exporter.ExportAnnotatorsForSeverity(severity, codeSmellGroup.ToList(),
                    "SanityCheck_" + codeSmell + "_Severity_");

                manovaTest.SetupTestArguments(
                    "D:/ccadet/annotations/sanity_check/Output/SanityCheck_" + codeSmell + "_Severity_" + severity + ".xlsx",
                    codeSmellGroup.First().MetricFeatures.Keys.ToList(), "Annotator");
                manovaTest.RunTest();
            }
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

            var exporter = new AnnotationConsistencyByMetricsExporter("D:/ccadet/annotations/sanity_check/Output/");
            var manovaTest = new ManovaTest.ManovaTest();

            var enumerator = instancesGroupedBySmells.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var codeSmellGroup = enumerator.Current;
                var codeSmell = codeSmellGroup.Key.Replace(" ", "_");
                
                exporter.ExportAnnotationsFromAnnotator(annotatorId, codeSmellGroup.ToList(),
                    "SanityCheck_" + codeSmell + "_Annotator_");

                manovaTest.SetupTestArguments(
                    "D:/ccadet/annotations/sanity_check/Output/SanityCheck_" + codeSmell + "_Annotator_" + annotatorId + ".xlsx",
                    codeSmellGroup.First().MetricFeatures.Keys.ToList(), "Annotation");
                manovaTest.RunTest();
            }
        }

        private static void ExportAnnotatedDataSet()
        {
            var instancesGroupedBySmells = GetAnnotatedInstancesGroupedBySmells(annotatorId: null);

            var exporter = new CompleteDataSetExporter("D:/ccadet/annotations/annotated/Output/");
            var enumerator = instancesGroupedBySmells.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var codeSmellGroup = enumerator.Current;
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

    }
}
