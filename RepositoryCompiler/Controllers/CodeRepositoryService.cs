using RepositoryCompiler.CodeModel;
using RepositoryCompiler.CodeModel.CaDETModel;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompiler.RepositoryAdapters;
using System.Collections.Generic;

namespace RepositoryCompiler.Controllers
{
    public class CodeRepositoryService
    {
        //This should be restructured. The codeRepoAdapter should be retrieved from a factory based on the information in the
        //RepositoryRepository which can be injected. In general this service should be reworked as it has many functions that might not be necessary.
        private readonly ICodeRepositoryAdapter _codeRepositoryAdapter;
        public CodeRepositoryService(ICodeRepositoryAdapter codeRepositoryAdapter)
        {
            _codeRepositoryAdapter = codeRepositoryAdapter;
        }

        public CodeRepositoryService() { }

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
            _codeRepositoryAdapter.CheckoutCommit(commit);
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);
            return factory.ParseFiles("C:/repo");
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

        public CaDETClass BuildClassModel(string sourceCode)
        {
            CodeModelFactory codeFactory = new CodeModelFactory(LanguageEnum.CSharp);
            return codeFactory.CreateCodeModel(sourceCode);
        }
    }
}
