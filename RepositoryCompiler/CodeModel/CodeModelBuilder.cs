using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using RepositoryCompiler.CodeModel.CaDETModel;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
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

        public List<CaDETClass> BuildCodeModel(IEnumerable<string> multipleClassSourceCode)
        {
            ICodeParser codeParser = SimpleParserFactory.CreateParser(_language);
            return codeParser.GetParsedClasses(multipleClassSourceCode);
        }

        public CaDETClass BuildCodeModel(string classSourceCode)
        {
            return BuildCodeModel(new List<string> { classSourceCode }).First();
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
    }
}
