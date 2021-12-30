using System.IO;
using LibGit2Sharp;

namespace DataSetExplorer.Infrastructure.RepositoryAdapters
{
    public class GitCodeRepository : ICodeRepository
    {
        public void CloneRepository(string url, string projectPath)
        {
            if(Directory.Exists(projectPath)) DeleteDirectory(projectPath);
            Directory.CreateDirectory(projectPath);
            Repository.Clone(url, projectPath);
        }

        public void CheckoutCommit(string commitHash, string projectPath)
        {
            Commands.Checkout(new Repository(projectPath), commitHash);
        }

        public void SetupRepository(string urlWithCommitHash, string projectPath)
        {
            var urlParts = urlWithCommitHash.Split("/tree/");
            var projectUrl = urlParts[0] + ".git";
            CloneRepository(projectUrl, projectPath);

            var commitHash = urlParts[1];
            CheckoutCommit(commitHash, projectPath);
        }

        private static void DeleteDirectory(string directory)
        {
            foreach (var subDirectory in Directory.EnumerateDirectories(directory))
            {
                DeleteDirectory(subDirectory);
            }

            foreach (var fileName in Directory.EnumerateFiles(directory))
            {
                var fileInfo = new FileInfo(fileName)
                {
                    Attributes = FileAttributes.Normal
                };
                fileInfo.Delete();
            }

            Directory.Delete(directory);
        }
    }
}
