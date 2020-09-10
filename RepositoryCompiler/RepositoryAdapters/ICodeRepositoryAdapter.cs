using System.Collections.Generic;
using RepositoryCompiler.CodeModel.CaDETModel;

namespace RepositoryCompiler.RepositoryAdapters
{
    public interface ICodeRepositoryAdapter
    {
        void CloneRepository();
        CaDETProject ParseProjectCode(CommitId commit);
        IEnumerable<CommitId> GetCommits(int numOfPreviousCommits);
        bool CheckForNewCommits();
    }
}
