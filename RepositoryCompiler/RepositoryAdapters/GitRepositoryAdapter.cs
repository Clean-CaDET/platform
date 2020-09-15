using System;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RepositoryCompiler.CodeModel.CaDETModel;

namespace RepositoryCompiler.RepositoryAdapters
{
    public class GitRepositoryAdapter : ICodeRepositoryAdapter
    {
        private readonly string _gitSourcePath;
        private readonly string _gitDestinationPath;
        private readonly string _mainBranchName;
        private readonly string _uname;
        private readonly string _pass;
        public GitRepositoryAdapter(Dictionary<string, string> settings)
        {
            _gitSourcePath = settings["CodeRepository:GitSourcePath"];
            _gitDestinationPath = settings["CodeRepository:GitDestinationPath"];
            _mainBranchName = settings["CodeRepository:MainBranchName"];
            _uname = settings.ContainsKey("CodeRepository:Username")
                ? settings["CodeRepository:Username"]
                : "TODO";
            _pass = settings.ContainsKey("CodeRepository:Password")
                ? settings["CodeRepository:Password"]
                : "TODO";
        }

        public void CloneRepository()
        {
            //Requires refactoring along with the rest of the configuration.
            //TODO: Rework once RepositoryRepository is established.
            var co = new CloneOptions
            {
                CredentialsProvider = (url, user, cred) => new UsernamePasswordCredentials { Username = _uname, Password = _pass }
            };
            Repository.Clone(_gitSourcePath, _gitDestinationPath, co);
        }

        public bool CheckForNewCommits()
        {
            CheckoutMasterBranch();
            CommitId localCommit = GetActiveCommit();
            PullChanges();
            bool changesOccurred = !localCommit.Equals(GetActiveCommit());
            return changesOccurred;
        }

        private void PullChanges()
        {
            //The current code only works with public repositories.
            //The nonsensical options below should be refactored and extracted into a function
            //That will be called by this function as well as CloneRepository.
            PullOptions options = new PullOptions
            {
                FetchOptions = new FetchOptions
                {
                    CredentialsProvider = (url, usernameFromUrl, types) =>
                        new UsernamePasswordCredentials() { Username = _uname, Password = _pass }
                }
            };
            var signature = new Signature(new Identity(_uname, _uname), DateTime.Now);

            Commands.Pull(GetRepository(), signature, options);
        }

        public CommitId GetActiveCommit()
        {
            return GetCommits(1).First();
        }
        
        public IEnumerable<CommitId> GetCommits(int numOfPreviousCommits)
        {
            return GetRepository().Commits.Take(numOfPreviousCommits).Select(commit => new CommitId(commit.Sha));
        }

        public CaDETProject ParseProjectCode(CommitId commit)
        {
            CheckoutCommit(commit);
            return new CaDETProject(_gitDestinationPath, ParseDocuments());
        }

        private IEnumerable<CaDETDocument> ParseDocuments()
        {
            //specific to C# - should extract to C# file identifier when appropriate
            string[] allFiles = Directory.GetFiles(_gitDestinationPath, "*.cs", SearchOption.AllDirectories);
            return allFiles.Select(s => new CaDETDocument(s, File.ReadAllText(s), LanguageEnum.CSharp));
        }

        private void CheckoutMasterBranch()
        {
            CheckoutCommit(null);
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
