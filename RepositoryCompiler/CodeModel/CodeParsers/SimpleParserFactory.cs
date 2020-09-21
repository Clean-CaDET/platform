using System.ComponentModel;
using RepositoryCompiler.CodeModel.CaDETModel;

namespace RepositoryCompiler.CodeModel.CodeParsers
{
    public class SimpleParserFactory
    {
        public static ICodeParser CreateParser(LanguageEnum language)
        {
            switch (language)
            {
                case LanguageEnum.CSharp: return new CSharpCodeParser();
                default: throw new InvalidEnumArgumentException();
            }
        }
    }
}
