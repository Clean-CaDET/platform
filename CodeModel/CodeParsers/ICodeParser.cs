using CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;

namespace CodeModel.CodeParsers
{
    public interface ICodeParser
    {
        List<CaDETClass> GetParsedClasses(IEnumerable<string> sourceCode);
    }
}