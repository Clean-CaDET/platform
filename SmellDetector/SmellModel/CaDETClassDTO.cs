using System;
using System.Collections.Generic;

namespace SmellDetector.SmellModel
{
    public class CaDETClassDTO
    {
        public string FullName { get; set; }
        public Dictionary<string, double> CodeSnippetMetrics { get; set; }

        public CaDETClassDTO(string fullName, Dictionary<string, double> codeSnippetMetrics)
        {
            FullName = fullName;
            CodeSnippetMetrics = codeSnippetMetrics;
        }

        public CaDETClassDTO()
        {
            CodeSnippetMetrics = new Dictionary<string, double>();
        }
    }
}
