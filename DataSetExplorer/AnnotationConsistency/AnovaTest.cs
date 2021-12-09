using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.DataSets.Model;
using DataSetExplorer.DataSetSerializer;
using FluentResults;

namespace DataSetExplorer.AnnotationConsistency
{
    public class AnovaTest : IMetricsSignificanceTester
    {
        private readonly string _anovaScriptFile = "./AnnotationConsistency/scripts/anova_test.py";
        private readonly string _pythonPath = "./AnnotationConsistency/venv/Scripts/python.exe";
        private string _annotatedInstancesFile;
        private string _dependentVariable;
        private string _independentVariable;
        private readonly string _annotatedInstancesFolderPath = "D:/ccadet/annotations/sanity_check/anova/Output/";

        private delegate Dictionary<string, string> TestCodeSmellDelegate(int id, List<Instance> instances, string codeSmell, List<CaDETMetric> metrics);

        public Result<Dictionary<string, Dictionary<string, string>>> TestForSingleAnnotator(int annotatorId, List<SmellCandidateInstances> instancesGroupedBySmells)
        {
            return Test(annotatorId, instancesGroupedBySmells, TestCodeSmellForAnnotator);
        }

        public Result<Dictionary<string, Dictionary<string, string>>> TestBetweenAnnotators(int severity, List<SmellCandidateInstances> instancesGroupedBySmells)
        {
            return Test(severity, instancesGroupedBySmells, TestCodeSmellBetweenAnnotators);
        }

        private Result<Dictionary<string, Dictionary<string, string>>> Test(int id, List<SmellCandidateInstances> instancesGroupedBySmells, TestCodeSmellDelegate testCodeSmell)
        {
            var results = new Dictionary<string, Dictionary<string, string>>();
            foreach (var codeSmellGroup in instancesGroupedBySmells)
            {
                var codeSmell = codeSmellGroup.CodeSmell.Name;
                var metrics = codeSmellGroup.Instances.First().MetricFeatures.Keys.ToList();
                var instances = codeSmellGroup.Instances;
                results[codeSmell] = testCodeSmell(id, instances, codeSmell, metrics);
            }
            return Result.Ok(results);
        }
       
        private Dictionary<string, string> TestCodeSmellForAnnotator(int annotatorId, List<Instance> instances, string codeSmell, List<CaDETMetric> metrics)
        {
            var results = new Dictionary<string, string>();
            string exportedAnnotationsFile = ExportAnnotationsForAnnotator(annotatorId, instances, codeSmell);
            foreach (var metric in metrics)
            {
                SetupTestArguments(exportedAnnotationsFile, metric, "Annotation");
                results[metric.ToString()] = StartProcess();
            }
            return results;
        }

        private Dictionary<string, string> TestCodeSmellBetweenAnnotators(int severity, List<Instance> instances, string codeSmell, List<CaDETMetric> metrics)
        {
            var results = new Dictionary<string, string>();
            string exportedAnnotationsFile = ExportAnnotationsForSeverity(severity, instances, codeSmell);
            foreach (var metric in metrics)
            {
                SetupTestArguments(exportedAnnotationsFile, metric, "Annotator");
                results[metric.ToString()] = StartProcess();
            }
            return results;
        }

        private string ExportAnnotationsForAnnotator(int annotatorId, List<Instance> instances, string codeSmell)
        {
            var exportedAnnotationsFile = "MetricsSignificance_" + codeSmell + "_Annotator_" + annotatorId;
            var exporter = new AnnotationConsistencyByMetricsExporter(_annotatedInstancesFolderPath);
            exporter.ExportAnnotationsFromAnnotator(annotatorId, instances, exportedAnnotationsFile);
            return exportedAnnotationsFile;
        }

        private string ExportAnnotationsForSeverity(int severity, List<Instance> instances, string codeSmell)
        {
            var exportedAnnotationsFile = "MetricsSignificance_" + codeSmell + "_Severity_" + severity;
            var exporter = new AnnotationConsistencyByMetricsExporter(_annotatedInstancesFolderPath);
            exporter.ExportAnnotatorsForSeverity(severity, instances, exportedAnnotationsFile);
            return exportedAnnotationsFile;
        }

        private void SetupTestArguments(string exportedAnnotationsFile, CaDETMetric metric, string independentVariable)
        {
            _annotatedInstancesFile = _annotatedInstancesFolderPath + exportedAnnotationsFile + ".xlsx";
            _dependentVariable = metric.ToString();
            _independentVariable = independentVariable;
        }

        private string StartProcess()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = _pythonPath,
                Arguments = $"{_anovaScriptFile} {_annotatedInstancesFile} \"{_dependentVariable}\" {_independentVariable}",
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
