using System.ComponentModel;
using RepositoryCompiler.CodeModel.CaDETModel;

namespace RepositoryCompiler.CodeModel.SyntaxParsers
{
    public class SimpleParserFactor
    {
        public static ISyntaxParser CreateParser(LanguageEnum language)
        {
            switch (language)
            {
                case LanguageEnum.CSharp: return new CSharpSyntaxParser();
                default: throw new InvalidEnumArgumentException();
            }
        }
    }
}
