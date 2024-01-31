using CodeModel.CaDETModel;
using System.Collections.Generic;

namespace CodeModel.CodeParsers
{
    public interface ICodeParser
    {
        CaDETProject Parse(IEnumerable<string> sourceCode);
        CaDETProject ParseWithPartial(IEnumerable<string> sourceCode);
    }
}