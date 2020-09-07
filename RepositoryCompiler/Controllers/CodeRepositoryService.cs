using RepositoryCompiler.CodeParsers.CaDETModel;
using RepositoryCompiler.CodeParsers.Data;
using RepositoryCompiler.RepositoryAdapters;

namespace RepositoryCompiler.Controllers
{
    public class CodeRepositoryService
    {
        private readonly ICodeRepositoryAdapter _codeRepositoryAdapter;

        public CodeRepositoryService(ICodeRepositoryAdapter codeRepositoryAdapter)
        {
            _codeRepositoryAdapter = codeRepositoryAdapter;
        }

        public void SetupRepository()
        {
            _codeRepositoryAdapter.CloneRepository();
        }

        public CaDETProject BuildModel(CommitId commit)
        {
            return _codeRepositoryAdapter.ParseProjectCode(commit);
        }
    }
}
