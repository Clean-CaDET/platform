namespace DataSetExplorer.Infrastructure.RepositoryAdapters
{
    public interface ICodeRepository
    {
        void CloneRepository(string url, string projectPath);
        void CheckoutCommit(string commitHash, string projectPath);
        void SetupRepository(string urlWithCommitHash, string projectPath);
    }
}
