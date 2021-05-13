using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;

namespace DataSetExplorer.ManovaTest
{
    class ManovaTest
    {
        private readonly string _manovaScriptFile = "../../../ManovaTest/manova_test.py";
        private readonly string _pythonPath = "../../../ManovaTest/venv/Scripts/python.exe";
        private string _annotatedInstancesFile;
        private string _metrics;
        private string _independentVariable;
        
        public void SetupTestArguments(string annotatedInstancesFile, List<CaDETMetric> metrics, string independentVariable)
        {
            _annotatedInstancesFile = annotatedInstancesFile;
            _metrics = "";
            foreach (var metric in metrics)
            {
                _metrics += metric + " + ";
            }
            _metrics = _metrics.Substring(0, _metrics.Length - 2);
            _independentVariable = independentVariable;
        }

        public void RunTest()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = _pythonPath;
            startInfo.Arguments = $"{_manovaScriptFile} {_annotatedInstancesFile} \"{_metrics}\" {_independentVariable}";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            using (Process process = Process.Start(startInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }
        }
    }
}
