using CodeModel.CodeParsers.CSharp;
using System.ComponentModel;

namespace CodeModel.CodeParsers
{
    internal class SimpleParserFactory
    {
        internal static ICodeParser CreateParser(LanguageEnum language)
        {
            switch (language)
            {
                case LanguageEnum.CSharp: return new CSharpCodeParser();
                default: throw new InvalidEnumArgumentException();
            }
        }
    }
}
