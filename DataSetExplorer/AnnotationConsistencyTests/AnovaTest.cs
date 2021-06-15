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
    public class AnovaTest : IMetricsSignificanceTester
    {
        private readonly string _anovaScriptFile = "../../../AnnotationConsistencyTests/anova_test.py";
        private readonly string _pythonPath = "../../../AnnotationConsistencyTests/venv/Scripts/python.exe";
        private string _annotatedInstancesFile;
        private string _dependentVariable;

        private string _independentVariable;
        private readonly string _annotatedInstancesFolderPath = "D:/ccadet/annotations/sanity_check/anova/Output/";

        public void Test(int annotatorId, IEnumerable<IGrouping<string, DataSetInstance>> instancesGroupedBySmells)
        {
            foreach (var codeSmellGroup in instancesGroupedBySmells)
            {
                var codeSmell = codeSmellGroup.Key.Replace(" ", "_");
                Console.Write(codeSmell);
                var metrics = codeSmellGroup.First().MetricFeatures.Keys.ToList();
                var instances = codeSmellGroup.ToList();
                RunTest(annotatorId, instances, codeSmell, metrics);
            }
        }

        public void RunTest(int annotatorId, List<DataSetInstance> instances, string codeSmell, List<CaDETMetric> metrics)
        {
            string exportedAnnotationsFile = ExportAnnotations(annotatorId, instances, codeSmell);
            foreach (var metric in metrics)
            {
                Console.Write("Metric " + metric);
                SetupTestArguments(exportedAnnotationsFile, metric, "Annotation");
                StartProcess();
            }
        }

        private string ExportAnnotations(int annotatorId, List<DataSetInstance> instances, string codeSmell)
        {
            var exportedAnnotationsFile = "SanityCheck_" + codeSmell + "_Annotator_" + annotatorId;
            var exporter = new AnnotationConsistencyByMetricsExporter(_annotatedInstancesFolderPath);
            exporter.ExportAnnotationsFromAnnotator(annotatorId, instances, exportedAnnotationsFile);
            return exportedAnnotationsFile;
        }

        private void SetupTestArguments(string exportedAnnotationsFile, CaDETMetric metric, string independentVariable)
        {
            _annotatedInstancesFile = _annotatedInstancesFolderPath + exportedAnnotationsFile + ".xlsx";
            _dependentVariable = metric.ToString();
            _independentVariable = independentVariable;
        }

        private void StartProcess()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = _pythonPath,
                Arguments = $"{_anovaScriptFile} {_annotatedInstancesFile} \"{_dependentVariable}\" {_independentVariable}",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            using Process process = Process.Start(startInfo);
            using StreamReader reader = process.StandardOutput;
            string result = reader.ReadToEnd();
            Console.Write(result);
        }
    }
}
