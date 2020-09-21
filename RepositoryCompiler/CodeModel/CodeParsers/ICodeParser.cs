using System.Collections.Generic;
using RepositoryCompiler.CodeModel.CaDETModel;

namespace RepositoryCompiler.CodeModel.CodeParsers
{
    public interface ICodeParser
    {
        List<CaDETClass> ParseClasses(string sourceCode);
        List<CaDETClass> CalculateSemanticMetrics(List<CaDETClass> classes);
    }
}