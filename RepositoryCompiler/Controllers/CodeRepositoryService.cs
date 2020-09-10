using RepositoryCompiler.CodeParsers.CaDETModel;
using RepositoryCompiler.RepositoryAdapters;
using System;
using System.Collections.Generic;

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

        public CaDETProject BuildProjectModel(string commitHash)
        {
            return BuildProjectModel(CommitId.Create(commitHash));
        }

        public CaDETProject BuildProjectModel(CommitId commit)
        {
            return _codeRepositoryAdapter.ParseProjectCode(commit);
        }

        public CaDETModel BuildModel(int numOfPreviousCommits)
        {
            IEnumerable<CommitId> previousCommits = _codeRepositoryAdapter.GetCommits(numOfPreviousCommits);
            CaDETModel model = new CaDETModel();
            foreach(CommitId commit in previousCommits)
            {
                model.ProjectHistory.Add(commit, BuildProjectModel(commit));
            }
            return model;
        }

        public bool UpdateRepository()
        {
            return _codeRepositoryAdapter.CheckForNewCommits();
        }
    }
}
