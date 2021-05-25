using CodeModel.CaDETModel.CodeItems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DataSetExplorer.AnnotationConsistencyTests
{
    class ManovaTest
    {
        private readonly string _manovaScriptFile = "../../../AnnotationConsistencyTests/manova_test.py";
        private readonly string _pythonPath = "../../../AnnotationConsistencyTests/venv/Scripts/python.exe";
        private string _annotatedInstancesFile;
        private string _metrics;
        private string _independentVariable;
        
        public void SetupTestArguments(string annotatedInstancesFile, List<CaDETMetric> metrics, string independentVariable)
        {
            _annotatedInstancesFile = annotatedInstancesFile;
            _metrics = string.Join(" + ", metrics.ToArray());
            _independentVariable = independentVariable;
        }

        public void RunTest()
        {
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
    }
}
