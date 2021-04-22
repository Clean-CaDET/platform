using System;
using System.Collections.Generic;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;

namespace SmellDetector.SmellModel
{
    public class CaDETClassDTO
    {
        public string FullName { get; set; }
        public Dictionary<CaDETMetric, double> CodeSnippetMetrics { get; set; }

        public CaDETClassDTO(string fullName, Dictionary<CaDETMetric, double> codeSnippetMetrics)
        {
            FullName = fullName;
            CodeSnippetMetrics = codeSnippetMetrics;
        }

        public CaDETClassDTO()
        {
            CodeSnippetMetrics = new Dictionary<CaDETMetric, double>();
        }
    }
}
