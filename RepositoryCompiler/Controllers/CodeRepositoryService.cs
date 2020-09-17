using RepositoryCompiler.RepositoryAdapters;
using System.Collections.Generic;
using RepositoryCompiler.CodeModel.CaDETModel;

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

        public IEnumerable<CaDETDocument> BuildProjectModel(string commitHash)
        {
            return BuildProjectModel(CommitId.Create(commitHash));
        }

        public IEnumerable<CaDETDocument> BuildProjectModel(CommitId commit)
        {
            return _codeRepositoryAdapter.ParseProjectCode(commit);
        }

        public CaDETModel BuildModel(int numOfPreviousCommits)
        {
            IEnumerable<CommitId> previousCommits = _codeRepositoryAdapter.GetCommits(numOfPreviousCommits);
            CaDETModel model = new CaDETModel();
            foreach(CommitId commit in previousCommits)
            {
                model.AddProject(commit, BuildProjectModel(commit));
            }
            return model;
        }

        public bool UpdateRepository()
        {
            return _codeRepositoryAdapter.CheckForNewCommits();
        }
    }
}
