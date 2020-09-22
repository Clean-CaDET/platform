using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using RepositoryCompiler.CodeModel.CaDETModel;
using RepositoryCompiler.CodeModel.CodeParsers;

namespace RepositoryCompiler.CodeModel
{
    public class CodeModelBuilder
    {
        private readonly LanguageEnum _language;

        public CodeModelBuilder(LanguageEnum language)
        {
            _language = language;
        }

        public CaDETProject ParseFiles(string sourceCodeLocation)
        {
            string[] allFiles = Directory.GetFiles(sourceCodeLocation, GetLanguageExtension(), SearchOption.AllDirectories);
            return new CaDETProject(LanguageEnum.CSharp, BuildCodeModel(allFiles.Select(File.ReadAllText)));
        }

        private string GetLanguageExtension()
        {
            switch (_language)
            {
                case LanguageEnum.CSharp: return "*.cs";
                default: throw new InvalidEnumArgumentException();
            }
        }

        public List<CaDETClass> BuildCodeModel(IEnumerable<string> codeTexts)
        {
            ICodeParser codeParser = SimpleParserFactory.CreateParser(_language);
            return codeParser.GetParsedClasses(codeTexts);
        }
    }
}
