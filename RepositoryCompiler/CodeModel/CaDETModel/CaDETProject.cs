using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETProject
    {
        public string Name { get; private set; }
        public IEnumerable<CaDETDocument> ProjectFiles { get; private set; }

        public CaDETProject(string name, IEnumerable<CaDETDocument> projectFiles)
        {
            Name = name;
            ProjectFiles = projectFiles;
        }
    }
}