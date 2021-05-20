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
            //ExportAnnotatedDataSet();
            //CheckAnnotationConsistencyForAnnotator(1);
            CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(1);
        }

        private static void CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(int severity)
        {
            var dataGroupedBySmells = GetAnnotationsAndMetricsGroupedBySmells(annotatorId: null);

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
                    codeSmellGroup.First().Item2.Keys.ToList(), "Annotator");
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
            var dataGroupedBySmells = GetAnnotationsAndMetricsGroupedBySmells(annotatorId);

            var exporter = new AnnotationConsistencyByMetricsExporter("D:/ccadet/annotations/sanity_check/Output/");
            var manovaTest = new ManovaTest.ManovaTest();

            var enumerator = dataGroupedBySmells.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var codeSmellGroup = enumerator.Current;
                var codeSmell = codeSmellGroup.Key.Replace(" ", "_");
                
                exporter.ExportAnnotationsFromAnnotator(annotatorId, codeSmellGroup.ToList(),
                    "SanityCheck_" + codeSmell + "_Annotator_");

                manovaTest.SetupTestArguments(
                    "D:/ccadet/annotations/sanity_check/Output/SanityCheck_" + codeSmell + "_Annotator_" + annotatorId + ".xlsx",
                    codeSmellGroup.First().Item2.Keys.ToList(), "Annotation");
                manovaTest.RunTest();
            }
        }

        private static void ExportAnnotatedDataSet()
        {
            var dataGroupedBySmells = GetAnnotationsAndMetricsGroupedBySmells(annotatorId: null);

            var exporter = new CompleteDataSetExporter("D:/ccadet/annotations/annotated/Output/");
            var enumerator = dataGroupedBySmells.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var codeSmellGroup = enumerator.Current;
                exporter.Export(codeSmellGroup.ToList(), "DataSet_" + codeSmellGroup.Key);
            }
        }

        private static IEnumerable<IGrouping<string, Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>>> GetAnnotationsAndMetricsGroupedBySmells(int? annotatorId)
        {
            var annotationsAndMetrics = new List<Tuple<DataSetInstance, Dictionary<CaDETMetric, double>>>();

            var projects = GetAnnotatedProjects();
            foreach (var key in projects.Keys)
            {
                CodeModelFactory factory = new CodeModelFactory();
                CaDETProject project = factory.CreateProjectWithCodeFileLinks(key.ToString());

                var annotatedInstances = LoadDataSet(projects[key].ToString()).GetAllInstances();
                LoadAnnotators(ref annotatedInstances);
                if (annotatorId != null) annotatedInstances = annotatedInstances.Where(i => i.IsAnnotatedBy((int)annotatorId)).ToList();
                var instanceMetrics = GetMetricsForExport(annotatedInstances, project);
                annotationsAndMetrics.AddRange(JoinAnnotationsAndMetrics(annotatedInstances, instanceMetrics));
            }
            return annotationsAndMetrics.GroupBy(t => t.Item1.Annotations.ToList()[0].InstanceSmell.Value);
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
