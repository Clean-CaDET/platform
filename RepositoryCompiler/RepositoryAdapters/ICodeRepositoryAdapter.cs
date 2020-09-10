using RepositoryCompiler.CodeParsers.CaDETModel;
using System.Collections.Generic;

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
