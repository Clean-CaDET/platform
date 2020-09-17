using RepositoryCompiler.CodeModel.SyntaxParsers;
using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETDocument
    {
        private readonly LanguageEnum _language;
        public string FilePath { get; }

        public IEnumerable<CaDETClass> Classes { get; private set; }
        
        public CaDETDocument(string filePath, string sourceCode, LanguageEnum language)
        {
            FilePath = filePath;
            _language = language;
            BuildCodeModel(sourceCode);
        }

        private void BuildCodeModel(string sourceCode)
        {
            Classes = SimpleParserFactor.CreateParser(_language).ParseClasses(sourceCode);
        }
    }
}