using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel.CodeItems
{
    public class CaDETLinkedType
    {
        public string FullType { get; set; }
        public List<CaDETClass> LinkedTypes { get; set; }
    }
}