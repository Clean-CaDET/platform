using System.Collections.Generic;

namespace RepositoryCompiler.CodeParsers.CaDETModel
{
    public class CaDETProject
    {
        public string Name { get; private set; }
        public IEnumerable<CaDETDocument> CompiledProjectItems { get; private set; }

        public CaDETProject(string name, IEnumerable<CaDETDocument> compiledProjectItems)
        {
            Name = name;
            CompiledProjectItems = compiledProjectItems;
        }
    }
}