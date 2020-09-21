using System;
using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETMethod
    {
        private int? _linesOfCode;
        public string Name { get; set; }
        public string SourceCode { get; set; }
        public bool IsConstructor { get; set; }
        public bool IsAccessor { get; set; }
        public CaDETClass Parent { get; set; }
        public int MetricCYCLO { get; set; }
        public List<CaDETMethod> InvokedMethods { get; set; }

        public int MetricLOC()
        {
            _linesOfCode ??= SourceCode.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
            return _linesOfCode.Value;
        }
    }
}