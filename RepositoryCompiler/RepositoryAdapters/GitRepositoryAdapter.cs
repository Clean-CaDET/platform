using LibGit2Sharp;
using Microsoft.Extensions.Configuration;
using RepositoryCompiler.CodeParsers.CaDETModel;
using RepositoryCompiler.CodeParsers.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RepositoryCompiler.RepositoryAdapters
{
    public class GitRepositoryAdapter : ICodeRepositoryAdapter
    {
        private readonly string _gitSourcePath;
        private readonly string _gitDestinationPath;
        private readonly string _mainBranchName;
        public GitRepositoryAdapter(IConfigurationSection settings)
        {
            _gitSourcePath = settings.GetSection("GitSourcePath").Value;
            _gitDestinationPath = settings.GetSection("GitDestinationPath").Value;
            _mainBranchName = settings.GetSection("MainBranchName").Value;
        }

        public void CloneRepository()
        {
            Repository.Clone(_gitSourcePath, _gitDestinationPath);
        }

        public IEnumerable<CommitId> GetCommits(int numOfPreviousCommits)
        {
            return GetRepository().Commits.Take(numOfPreviousCommits).Select(commit => new CommitId(commit.Sha));
        }

        public CaDETProject ParseProjectCode(CommitId commit)
        {
            CheckoutCommit(commit);
            //specific to C# - should extract to C# file identifier when appropriate
            string[] allFiles = Directory.GetFiles(_gitDestinationPath, "*.cs", SearchOption.AllDirectories);
            
            return new CaDETProject(_gitDestinationPath, allFiles.Select(s => new CaDETDocument(s, File.ReadAllText(s))));
        }

        private void CheckoutCommit(CommitId commit)
        {
            if (commit != null) Commands.Checkout(GetRepository(), commit.Hash);
            else Commands.Checkout(GetRepository(), _mainBranchName);
        }

        private Repository GetRepository()
        {
            return new Repository(_gitDestinationPath);
        }
    }
}
