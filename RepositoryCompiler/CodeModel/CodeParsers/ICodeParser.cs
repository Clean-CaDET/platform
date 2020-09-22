using System.Collections.Generic;
using RepositoryCompiler.CodeModel.CaDETModel;

namespace RepositoryCompiler.CodeModel.CodeParsers
{
    public interface ICodeParser
    {
        List<CaDETClass> GetParsedClasses(IEnumerable<string> sourceCode);
    }
}