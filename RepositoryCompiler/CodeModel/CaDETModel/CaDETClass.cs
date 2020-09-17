using System;
using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETClass
    {
        private int? _linesOfCode;
        public string Name { get; set; }
        public string FullName { get; set; }
        public string SourceCode { get; set; }
        public List<CaDETMethod> Methods { get; set; }
        public List<CaDETField> Fields { get; set; }

        public int GetMetricLOC()
        {
            _linesOfCode ??= SourceCode.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
            return _linesOfCode.Value;
        }

        public int GetMetricNMD()
        {
            return Methods.Count;
        }

        public int GetMetricNAD()
        {
            return Fields.Count;
        }
    }
}