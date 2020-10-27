using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel.CodeItems
{
    public class CaDETField
    {
        public string Name { get; internal set; }
        public List<CaDETModifier> Modifiers { get; internal set; }
        public CaDETClass Parent { get; internal set; }
    }
}