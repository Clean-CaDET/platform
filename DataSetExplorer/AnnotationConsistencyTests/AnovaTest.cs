using CodeModel.CaDETModel.CodeItems;
using System;
using System.Diagnostics;
using System.IO;

namespace DataSetExplorer.AnnotationConsistencyTests
{
    class AnovaTest
    {
        private readonly string _anovaScriptFile = "../../../AnnotationConsistencyTests/anova_test.py";
        private readonly string _pythonPath = "../../../AnnotationConsistencyTests/venv/Scripts/python.exe";
        private string _annotatedInstancesFile;
        private string _metric;
        private string _independentVariable;
        
        public void SetupTestArguments(string annotatedInstancesFile, CaDETMetric metric, string independentVariable)
        {
            _annotatedInstancesFile = annotatedInstancesFile;
            _metric = metric.ToString();
            _independentVariable = independentVariable;
        }

        public void RunTest()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = _pythonPath,
                Arguments = $"{_anovaScriptFile} {_annotatedInstancesFile} \"{_metric}\" {_independentVariable}",
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
