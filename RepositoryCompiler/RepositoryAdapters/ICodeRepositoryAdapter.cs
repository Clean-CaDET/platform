using System.Collections.Generic;

namespace RepositoryCompiler.RepositoryAdapters
{
    public interface ICodeRepositoryAdapter
    {
        void CloneRepository(string gitURL, string username, string password, string dirName);
        void CloneRepository();
        void CheckoutCommit(CommitId commit);
        IEnumerable<CommitId> GetCommits(int numOfPreviousCommits);
        bool CheckForNewCommits();
    }
}
