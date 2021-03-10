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
        public void Clones_repository_with_params()
        {
            ICodeRepositoryAdapter gitAdapter = new GitRepositoryAdapter();

            gitAdapter.CloneRepository("https://github.com/Ana00000/Challenge-inspiration.git", "GGojko", "gojkoG9G8G7", "ActiveEducation");

            Directory.Exists("C:" + _separator + "ActiveEducation").ShouldBeTrue();
        }

        [Fact]
        public void Clones_repository()
        {
            CleanTestDirectory();
            ICodeRepositoryAdapter gitAdapter = new GitRepositoryAdapter(GetTestConfiguration());

            gitAdapter.CloneRepository();

            GitDirectoryExists().ShouldBeTrue();
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Checks_presence_of_file_on_commit(CommitId commit, bool isFilePresent)
        {
            ICodeRepositoryAdapter gitAdapter = new GitRepositoryAdapter(GetTestConfiguration());
            string fileName = GetTestPath() + _separator + "LibGit2Sharp" + _separator + "BlameOptions.cs";

            gitAdapter.CheckoutCommit(commit);

            File.Exists(fileName).ShouldBe(isFilePresent);
        }

        public static IEnumerable<object[]> Data()
        {
            var retVal = new List<object[]>();

            retVal.Add(new object[] { new CommitId("a3f95fc9e92aa4bec32f4c4a535b0316ec2ea470"), false });
            retVal.Add(new object[] { null, true });

            return retVal;

        }

        [Fact]
        public void Checks_content_of_commit()
        {
            ICodeRepositoryAdapter gitAdapter = new GitRepositoryAdapter(GetTestConfiguration());
            string fileName = GetTestPath() + _separator + "LibGit2Sharp" + _separator + "BlameOptions.cs";

            gitAdapter.CheckoutCommit(new CommitId("a3f95fc9e92aa4bec32f4c4a535b0316ec2ea470"));

            File.Exists(fileName).ShouldBe(false);
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
            if (GitDirectoryExists()) Directory.Delete(testPath, true);
            Directory.CreateDirectory(testPath);
        }

        private string GetTestPath()
        {
            return "C:" + _separator + "repo";
        }
    }
}