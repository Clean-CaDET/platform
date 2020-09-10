using System.Collections.Generic;
using RepositoryCompiler.CodeModel.CaDETModel;

namespace RepositoryCompiler.CodeModel.SyntaxParsers
{
    public interface ISyntaxParser
    {
        IEnumerable<CaDETClass> ParseClasses(string sourceCode);
    }
}