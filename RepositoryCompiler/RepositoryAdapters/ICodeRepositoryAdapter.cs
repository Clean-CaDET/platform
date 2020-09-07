using RepositoryCompiler.CodeParsers.CaDETModel;
using RepositoryCompiler.CodeParsers.Data;

namespace RepositoryCompiler.RepositoryAdapters
{
    public interface ICodeRepositoryAdapter
    {
        void CloneRepository();
        CaDETProject ParseProjectCode(CommitId commit);
    }
}
