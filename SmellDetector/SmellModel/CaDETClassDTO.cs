using System;
using System.Collections.Generic;

namespace SmellDetector.SmellModel
{
    public class CaDETClassDTO
    {
        public string FullName { get; set; }
        public Dictionary<string, double> CodeItemMetrics { get; set; }

        public CaDETClassDTO(string fullName, Dictionary<string, double> codeItemMetrics)
        {
            FullName = fullName;
            CodeItemMetrics = codeItemMetrics;
        }

        public CaDETClassDTO()
        {
            CodeItemMetrics = new Dictionary<string, double>();
        }
    }
}
