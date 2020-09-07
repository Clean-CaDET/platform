using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using Microsoft.Extensions.Configuration;
using RepositoryCompiler.CodeParsers.CaDETModel;
using RepositoryCompiler.CodeParsers.Data;

namespace RepositoryCompiler.RepositoryAdapters
{
    public class GitRepositoryAdapter : ICodeRepositoryAdapter
    {
        private readonly string _gitSourcePath;
        private readonly string _gitDestinationPath;
        public GitRepositoryAdapter(IConfigurationSection settings)
        {
            _gitSourcePath = settings.GetSection("GitSourcePath").Value;
            _gitDestinationPath = settings.GetSection("GitDestinationPath").Value;
        }

        public void CloneRepository()
        {
            Repository.Clone(_gitSourcePath, _gitDestinationPath);
        }

        public CaDETProject ParseProjectCode(CommitId commit)
        {
            if(commit != null) CheckoutCommit(commit);
            //specific to C# - should extract to C# language compiler when appropriate
            string[] allFiles = Directory.GetFiles(_gitDestinationPath, "*.cs", SearchOption.AllDirectories);
            
            return new CaDETProject(_gitDestinationPath, allFiles.Select(s => new CaDETDocument(s, File.ReadAllText(s))));
        }

        private void CheckoutCommit(CommitId commit)
        {
            var repo = new Repository(_gitDestinationPath);
            Commands.Checkout(repo, commit.Hash);
        }
    }
}
