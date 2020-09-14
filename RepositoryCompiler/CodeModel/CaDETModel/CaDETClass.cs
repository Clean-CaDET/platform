using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETClass
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string SourceCode { get; set; }
        public List<CaDETMethod> Methods { get; set; }
        public List<CaDETField> Fields { get; set; }
    }
}