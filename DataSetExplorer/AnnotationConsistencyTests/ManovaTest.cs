using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.DataSetSerializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DataSetExplorer.AnnotationConsistencyTests
{
    public class ManovaTest
    {
        private readonly string _manovaScriptFile = "../../../AnnotationConsistencyTests/manova_test.py";
        private readonly string _pythonPath = "../../../AnnotationConsistencyTests/venv/Scripts/python.exe";
        private string _annotatedInstancesFile;
        private string _metrics;
        private string _independentVariable;
        private readonly string _annotatedInstancesFolderPath = "D:/ccadet/annotations/sanity_check/Output/";

        public delegate void TestDelegate(int id, List<DataSetInstance> instances, string codeSmell, List<CaDETMetric> metrics);

        public void Run(IEnumerable<IGrouping<string, DataSetInstance>> instancesGroupedBySmells,
            int id, TestDelegate test)
        {
            foreach (var codeSmellGroup in instancesGroupedBySmells)
            {
                var codeSmell = codeSmellGroup.Key.Replace(" ", "_");
                var metrics = codeSmellGroup.First().MetricFeatures.Keys.ToList();
                var instances = codeSmellGroup.ToList();

                test(id, instances, codeSmell, metrics);
            }
        }

        public void TestForMultipleAnnotators(int severity, List<DataSetInstance> instances, string codeSmell, List<CaDETMetric> metrics)
        {
            string exportedAnnotatorsFile = ExportAnnotatorsForSeverity(severity, instances, codeSmell);
            SetupTestArguments(exportedAnnotatorsFile, metrics, "Annotator");
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = _pythonPath,
                Arguments = $"{_manovaScriptFile} {_annotatedInstancesFile} \"{_metrics}\" {_independentVariable}",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            using Process process = Process.Start(startInfo);
            using StreamReader reader = process.StandardOutput;
            string result = reader.ReadToEnd();
            Console.Write(result);
        }

        private string ExportAnnotatorsForSeverity(int severity, List<DataSetInstance> instances, string codeSmell)
        {
            var exportedAnnotatorsFile = "SanityCheck_" + codeSmell + "_Severity_" + severity;
            var exporter = new AnnotationConsistencyByMetricsExporter(_annotatedInstancesFolderPath);
            exporter.ExportAnnotatorsForSeverity(severity, instances, exportedAnnotatorsFile);
            return exportedAnnotatorsFile;
        }

        public void TestForSingleAnnotator(int annotatorId, List<DataSetInstance> instances, string codeSmell, List<CaDETMetric> metrics)
        {
            string exportedAnnotationsFile = ExportAnnotationsForSingleAnnotator(annotatorId, instances, codeSmell);
            SetupTestArguments(exportedAnnotationsFile, metrics, "Annotation");
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = _pythonPath,
                Arguments = $"{_manovaScriptFile} {_annotatedInstancesFile} \"{_metrics}\" {_independentVariable}",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            using Process process = Process.Start(startInfo);
            using StreamReader reader = process.StandardOutput;
            string result = reader.ReadToEnd();
            Console.Write(result);
        }

        private string ExportAnnotationsForSingleAnnotator(int annotatorId, List<DataSetInstance> instances, string codeSmell)
        {
            var exportedAnnotationsFile = "SanityCheck_" + codeSmell + "_Annotator_" + annotatorId;
            var exporter = new AnnotationConsistencyByMetricsExporter(_annotatedInstancesFolderPath);
            exporter.ExportAnnotationsFromAnnotator(annotatorId, instances, exportedAnnotationsFile);
            return exportedAnnotationsFile;
        }

        private void SetupTestArguments(string annotatedInstancesFile, List<CaDETMetric> metrics, string independentVariable)
        {
            _annotatedInstancesFile = _annotatedInstancesFolderPath + annotatedInstancesFile + ".xlsx";
            _metrics = string.Join(" + ", metrics.ToArray());
            _independentVariable = independentVariable;
        }
    }
}
