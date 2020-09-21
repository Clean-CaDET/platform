using RepositoryCompiler.RepositoryAdapters;
using Shouldly;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace RepositoryCompilerTests.Integration
{
    public class GitRepositoryAdapterTests
    {
        private readonly char _separator = Path.DirectorySeparatorChar;

        [Fact]
        public void Clones_repository()
        {
            CleanTestDirectory();
            ICodeRepositoryAdapter gitAdapter = new GitRepositoryAdapter(GetTestConfiguration());

            gitAdapter.CloneRepository();

            GitDirectoryExists().ShouldBeTrue();
        }

        [Fact]
        public void Checks_content_of_master_commit()
        {
            ICodeRepositoryAdapter gitAdapter = new GitRepositoryAdapter(GetTestConfiguration());
            string presentFile = GetTestPath() + _separator + "LibGit2Sharp" + _separator + "BlameOptions.cs";

            gitAdapter.CheckoutCommit(null);

            File.Exists(presentFile).ShouldBeTrue();
        }

        [Fact]
        public void Checks_content_of_commit()
        {
            ICodeRepositoryAdapter gitAdapter = new GitRepositoryAdapter(GetTestConfiguration());
            string missingFile = GetTestPath() + _separator + "LibGit2Sharp" + _separator + "BlameOptions.cs";

            gitAdapter.CheckoutCommit(new CommitId("a3f95fc9e92aa4bec32f4c4a535b0316ec2ea470"));

            File.Exists(missingFile).ShouldBeFalse();
        }

        [Fact]
        public void Finds_no_new_commits()
        {
            ICodeRepositoryAdapter gitAdapter = new GitRepositoryAdapter(GetTestConfiguration());

            bool pulledNewCommits = gitAdapter.CheckForNewCommits();

            pulledNewCommits.ShouldBeFalse();
        }

        private bool GitDirectoryExists()
        {
            return Directory.Exists(GetTestPath() + _separator + ".git");
        }

        private Dictionary<string, string> GetTestConfiguration()
        {
            //TODO: Move test data to JSON or similar.
            return new Dictionary<string, string>()
            {
                { "CodeRepository:GitSourcePath", "https://github.com/clean-cadet-ftn/git-test-repo.git" },
                { "CodeRepository:GitDestinationPath", GetTestPath() },
                { "CodeRepository:MainBranchName", "master" },
                //SENSITIVE
                { "CodeRepository:Username", "clean-cadet-ftn" }
            };
        }

        private void CleanTestDirectory()
        {
            var testPath = GetTestPath();
            if(GitDirectoryExists()) Directory.Delete(testPath, true);
            Directory.CreateDirectory(testPath);
        }

        private string GetTestPath()
        {
            return "C:" + _separator + "CaDETTests";
        }
    }
}