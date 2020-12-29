using System;
using RepositoryCompiler.CodeModel;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using Shouldly;
using System.Linq;
using Xunit;

namespace RepositoryCompilerTests.Integration
{
    public class CaDETModelBuilderTests
    {
        [Fact]
        public void Build_code_model_from_repository()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);

            var project = factory.CreateProject("C:/repo");

            project.Classes.ShouldNotBeEmpty();
            CaDETClass conflict = project.Classes.Find(c => c.FullName.Equals("LibGit2Sharp.Conflict"));
            conflict.ShouldNotBeNull();
            conflict.Fields.ShouldContain(f => f.Name.Equals("ancestor"));
            conflict.Fields.ShouldContain(f => f.Name.Equals("ours"));
            conflict.Fields.ShouldContain(f => f.Name.Equals("theirs"));
            conflict.Members.ShouldContain(m =>
                m.Name.Equals("Conflict") && m.Type.Equals(CaDETMemberType.Constructor) && m.AccessedFields.Count == 3);
            conflict.Members.ShouldContain(m =>
                m.Name.Equals("Equals") && m.Type.Equals(CaDETMemberType.Method) && m.AccessedFields.Count == 1);
            conflict.Metrics.LOC.ShouldBe(108);
            conflict.Metrics.LCOM.ShouldBe(0.833);
            conflict.Metrics.NAD.ShouldBe(4);
            conflict.Metrics.NMD.ShouldBe(3);
            conflict.Metrics.WMC.ShouldBe(8);
            CaDETClass certificate = project.Classes.Find(c => c.FullName.Equals("LibGit2Sharp.Certificate"));
            certificate.ShouldNotBeNull();
            certificate.Members.ShouldBeEmpty();
            certificate.Fields.ShouldBeEmpty();
            certificate.Metrics.LCOM.ShouldBeNull();
            certificate.Metrics.LOC.ShouldBe(3);
            certificate.Metrics.NAD.ShouldBe(0);
            certificate.Metrics.NMD.ShouldBe(0);
            certificate.Metrics.WMC.ShouldBe(0);
            CaDETClass handles =
                project.Classes.Find(c => c.FullName.Equals("LibGit2Sharp.Core.Handles.Libgit2Object"));
            handles.ShouldNotBeNull();
            handles.Fields.ShouldContain(f => f.Name.Equals("ptr"));
            handles.Members.ShouldContain(m => m.Name.Equals("Handle") && m.Type.Equals(CaDETMemberType.Property));
            handles.Members.ShouldContain(m => m.Name.Equals("Dispose") && m.Type.Equals(CaDETMemberType.Method)
                                                                        && m.InvokedMethods.Count == 1 &&
                                                                        m.AccessedFields.Count == 0 &&
                                                                        m.AccessedAccessors.Count == 0);
            handles.Metrics.LCOM.ShouldBe(0.667);
            handles.Metrics.LOC.ShouldBe(100);
            handles.Metrics.NAD.ShouldBe(3);
            handles.Metrics.NMD.ShouldBe(4);
            handles.Metrics.WMC.ShouldBe(10);
        }

        [Theory]
        [InlineData("C:/test-repo")]
        [InlineData("C:/sdataset")]
        [InlineData("C:/sdataset2")]
        [InlineData("C:/sdataset3")]
        [InlineData("C:/sdataset4")]
        [InlineData("C:/sdataset5")]
        public void Create_code_model_with_links(string folderLocation)
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);
            var project = factory.CreateProjectWithCodeFileLinks(folderLocation);

            var classes = project.Classes.Select(c => c.FullName);
            var members = project.Classes.SelectMany(c => c.Members).Select(m => m.Signature());
            var keys = project.CodeLinks.Keys.ToList();
            var difference = classes.Union(members).Except(keys);

            difference.ShouldBeEmpty();
        }

        [Fact]
        public void Create_code_model_with_links_on_large_repo()
        {
            CodeModelFactory factory = new CodeModelFactory(LanguageEnum.CSharp);
            var project = factory.CreateProjectWithCodeFileLinks("C:/repo");

            var projectClassesAndMembersCount = project.Classes.Count + project.Classes.Sum(c => c.Members.Count);

            project.CodeLinks.Count.ShouldBe(projectClassesAndMembersCount);
            project.CodeLinks.TryGetValue("LibGit2Sharp.ObjectDatabase", out var locationLink);
            locationLink.StartLoC.ShouldBe(17);
            locationLink.EndLoC.ShouldBe(1091);
            project.CodeLinks.TryGetValue("LibGit2Sharp.ObjectDatabase.RevertCommit(LibGit2Sharp.Commit, LibGit2Sharp.Commit, int, LibGit2Sharp.MergeTreeOptions)", out locationLink);
            locationLink.StartLoC.ShouldBe(1026);
            locationLink.EndLoC.ShouldBe(1090);
            project.CodeLinks.TryGetValue("LibGit2Sharp.MergeTreeOptions.MergeTreeOptions()", out locationLink);
            locationLink.StartLoC.ShouldBe(15);
            locationLink.EndLoC.ShouldBe(16);
        }
    }
}
