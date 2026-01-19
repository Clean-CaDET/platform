using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using DataSetExplorer.Infrastructure.RepositoryAdapters;
using Xunit;

namespace DataSetExplorer.Tests.Integration
{
    public class GitRepositoryTests
    {
        private readonly char _separator = Path.DirectorySeparatorChar;

        [Fact]
        public void Clones_repository()
        {
            ICodeRepository gitAdapter = new GitCodeRepository();

            gitAdapter.CloneRepository("https://github.com/clean-cadet-ftn/git-test-repo.git", GetTestPath());

            GitDirectoryExists().ShouldBeTrue();
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Checks_presence_of_file_on_commit(string commit, bool isFilePresent)
        {
            ICodeRepository gitAdapter = new GitCodeRepository();
            var fileName = GetTestPath() + _separator +  "LibGit2Sharp" + _separator + "BlameOptions.cs";

            gitAdapter.CheckoutCommit(commit, GetTestPath());

            File.Exists(fileName).ShouldBe(isFilePresent);
        }

        public static IEnumerable<object[]> Data()
        {
            return new List<object[]>
            {
                new object[] { "a3f95fc9e92aa4bec32f4c4a535b0316ec2ea470", false },
                new object[] { "7fc4be5193dbdd08538b4b150332b5a73770e0f6", true }
            };
        }
        
        private bool GitDirectoryExists()
        {
            return Directory.Exists(GetTestPath() + _separator + ".git");
        }

        private string GetTestPath()
        {
            return Path.GetPathRoot(Environment.SystemDirectory) + "test-repository" + _separator + "TestRepo";
        }
    }
}
