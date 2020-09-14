using RepositoryCompiler.CodeModel.SyntaxParsers;
using System.Collections.Generic;

namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETDocument
    {
        public string FilePath { get; private set; }
        
        private readonly string _sourceCode;
        private readonly LanguageEnum _language;

        public IEnumerable<CaDETClass> Classes { get; private set; }
        
        public CaDETDocument(string filePath, string sourceCode, LanguageEnum language)
        {
            FilePath = filePath;
            _sourceCode = sourceCode;
            _language = language;
            BuildSyntaxModel();
        }

        public void BuildSyntaxModel()
        {
            Classes = SimpleParserFactor.CreateParser(_language).ParseClasses(_sourceCode);
        }
    }
}