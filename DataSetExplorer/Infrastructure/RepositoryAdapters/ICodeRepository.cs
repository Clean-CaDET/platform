using AutoMapper.Configuration;

namespace DataSetExplorer.Infrastructure.RepositoryAdapters
{
    public interface ICodeRepository
    {
        void CloneRepository(string url, string projectPath, string gitUser, string gitToken);
        void CheckoutCommit(string commitHash, string projectPath);
        void SetupRepository(string urlWithCommitHash, string projectPath, string gitUser, string gitToken, string environmentType);
    }
}
