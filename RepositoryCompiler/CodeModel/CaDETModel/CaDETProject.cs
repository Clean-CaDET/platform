using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETProject
    {
        private readonly LanguageEnum _language;

        public List<CaDETClass> Classes { get; private set;  }
        
        public CaDETProject(LanguageEnum language, List<CaDETClass> classes)
        {
            _language = language;
            Classes = classes;
        }
    }
}