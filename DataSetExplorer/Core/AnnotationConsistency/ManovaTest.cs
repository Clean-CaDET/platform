using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.Core.DataSetSerializer;
using FluentResults;

namespace DataSetExplorer.Core.AnnotationConsistency
{
    public class ManovaTest : IAnnotatorsConsistencyTester
    {
        private readonly string _manovaScriptFile = "./Core/AnnotationConsistency/scripts/manova_test.py";
        private readonly string _pythonPath = "./Core/AnnotationConsistency/venv/Scripts/python.exe";
        private string _annotatedInstancesFile;
        private string _dependentVariable;
        private string _independentVariable;
        private readonly string _annotatedInstancesFolderPath = "D:/ccadet/annotations/sanity_check/Output/";

        private delegate void PrepareTestDelegate(int id, List<Instance> instances, string codeSmell, List<CaDETMetric> metrics);

        public Result<Dictionary<string, string>> TestConsistencyBetweenAnnotators(string severity, List<SmellCandidateInstances> instancesGroupedBySmells)
        {
            var results = new Dictionary<string, string>();
            foreach (var codeSmellGroup in instancesGroupedBySmells)
            {
                var codeSmell = codeSmellGroup.CodeSmell.Name;
                var metrics = codeSmellGroup.Instances.First().MetricFeatures.Keys.ToList();
                var instances = codeSmellGroup.Instances;

                PrepareDataForBetweenAnnotators(severity, instances, codeSmell, metrics);
                results[codeSmell] = StartProcess();
            }
            return Result.Ok(results);
        }

        public Result<Dictionary<string, string>> TestConsistencyOfSingleAnnotator(int annotatorId, List<SmellCandidateInstances> instancesGroupedBySmells)
        {
            var results = new Dictionary<string, string>();
            foreach (var codeSmellGroup in instancesGroupedBySmells)
            {
                var codeSmell = codeSmellGroup.CodeSmell.Name;
                var metrics = codeSmellGroup.Instances.First().MetricFeatures.Keys.ToList();
                var instances = codeSmellGroup.Instances;

                PrepareDataForSingleAnnotator(annotatorId, instances, codeSmell, metrics);
                results[codeSmell] = StartProcess();
            }
            return Result.Ok(results);
        }

        private void PrepareDataForBetweenAnnotators(string severity, List<Instance> instances, string codeSmell, List<CaDETMetric> metrics)
        {
            string exportedAnnotatorsFile = ExportAnnotatorsForSeverity(severity, instances, codeSmell);
            SetupTestArguments(exportedAnnotatorsFile, metrics, "Annotator");
        }
        private void PrepareDataForSingleAnnotator(int annotatorId, List<Instance> instances, string codeSmell, List<CaDETMetric> metrics)
        {
            string exportedAnnotationsFile = ExportAnnotationsForSingleAnnotator(annotatorId, instances, codeSmell);
            SetupTestArguments(exportedAnnotationsFile, metrics, "Annotation");
        }

        private string ExportAnnotatorsForSeverity(string severity, List<Instance> instances, string codeSmell)
        {
            var exportedAnnotatorsFile = "SanityCheck_" + codeSmell + "_Severity_" + severity;
            var exporter = new AnnotationConsistencyByMetricsExporter(_annotatedInstancesFolderPath);
            exporter.ExportAnnotatorsForSeverity(severity, instances, exportedAnnotatorsFile);
            return exportedAnnotatorsFile;
        }

        private string ExportAnnotationsForSingleAnnotator(int annotatorId, List<Instance> instances, string codeSmell)
        {
            var exportedAnnotationsFile = "SanityCheck_" + codeSmell + "_Annotator_" + annotatorId;
            var exporter = new AnnotationConsistencyByMetricsExporter(_annotatedInstancesFolderPath);
            exporter.ExportAnnotationsFromAnnotator(annotatorId, instances, exportedAnnotationsFile);
            return exportedAnnotationsFile;
        }

        private void SetupTestArguments(string annotatedInstancesFile, List<CaDETMetric> metrics, string independentVariable)
        {
            _annotatedInstancesFile = _annotatedInstancesFolderPath + annotatedInstancesFile + ".xlsx";
            _dependentVariable = string.Join(" + ", metrics.ToArray());
            _independentVariable = independentVariable;
        }

        private string StartProcess()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = _pythonPath,
                Arguments = $"{_manovaScriptFile} {_annotatedInstancesFile} \"{_dependentVariable}\" {_independentVariable}",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            Process process = Process.Start(startInfo);
            StreamReader reader = process.StandardOutput;
            var result = reader.ReadToEnd();
            return result.Equals("") ? "Unable to calculate result, because there is not enough data to conduct a statistical test." : result;
        }
    }
}
