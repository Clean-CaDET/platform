using System.Collections.Generic;
using RepositoryCompiler.CodeModel.CaDETModel;

namespace RepositoryCompiler.RepositoryAdapters
{
    public interface ICodeRepositoryAdapter
    {
        void CloneRepository();
        IEnumerable<CaDETDocument> ParseProjectCode(CommitId commit);
        IEnumerable<CommitId> GetCommits(int numOfPreviousCommits);
        bool CheckForNewCommits();
    }
}
