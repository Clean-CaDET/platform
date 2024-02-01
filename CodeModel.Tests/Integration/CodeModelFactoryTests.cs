﻿using CodeModel.CaDETModel.CodeItems;
using Shouldly;
using System.Linq;
using Xunit;

namespace CodeModel.Tests.Integration
{
    public class CodeModelFactoryTests
    {
        [Fact]
        public void Build_code_model_from_repository()
        {
            CodeModelFactory factory = new CodeModelFactory();

            var project = factory.CreateProject("C:/repo");

            project.Classes.ShouldNotBeEmpty();
            CaDETClass conflict = project.Classes.Find(c => c.FullName.Equals("LibGit2Sharp.Conflict"));
            conflict.ShouldNotBeNull();
            conflict.Fields.ShouldContain(f => f.Name.Equals("ancestor"));
            conflict.Fields.ShouldContain(f => f.Name.Equals("ours"));
            conflict.Fields.ShouldContain(f => f.Name.Equals("theirs"));
            conflict.Members.ShouldContain(m =>
                m.Name.Equals("Conflict") && m.Type.Equals(CaDETMemberType.Constructor) && m.AccessedFields.Distinct().Count() == 3);
            conflict.Members.ShouldContain(m =>
                m.Name.Equals("Equals") && m.Type.Equals(CaDETMemberType.Method) && m.AccessedFields.Distinct().Count() == 1);
            conflict.Metrics[CaDETMetric.CLOC].ShouldBe(108);
            conflict.Metrics[CaDETMetric.LCOM].ShouldBe(0.833);
            conflict.Metrics[CaDETMetric.NAD].ShouldBe(4);
            conflict.Metrics[CaDETMetric.NMD].ShouldBe(3);
            conflict.Metrics[CaDETMetric.WMC].ShouldBe(8);
            CaDETClass certificate = project.Classes.Find(c => c.FullName.Equals("LibGit2Sharp.Certificate"));
            certificate.ShouldNotBeNull();
            certificate.Members.ShouldBeEmpty();
            certificate.Fields.ShouldBeEmpty();
            certificate.Metrics[CaDETMetric.LCOM].ShouldBe(-1);
            certificate.Metrics[CaDETMetric.CLOC].ShouldBe(3);
            certificate.Metrics[CaDETMetric.NAD].ShouldBe(0);
            certificate.Metrics[CaDETMetric.NMD].ShouldBe(0);
            certificate.Metrics[CaDETMetric.WMC].ShouldBe(0);
            CaDETClass handles =
                project.Classes.Find(c => c.FullName.Equals("LibGit2Sharp.Core.Handles.Libgit2Object"));
            handles.ShouldNotBeNull();
            handles.Fields.ShouldContain(f => f.Name.Equals("ptr"));
            handles.Members.ShouldContain(m => m.Name.Equals("Handle") && m.Type.Equals(CaDETMemberType.Property));
            handles.Members.ShouldContain(m => m.Name.Equals("Dispose") && m.Type.Equals(CaDETMemberType.Method)
                                                                        && m.InvokedMethods.Distinct().Count() == 1 &&
                                                                        m.AccessedFields.Count == 0 &&
                                                                        m.AccessedAccessors.Count == 0);
            handles.Metrics[CaDETMetric.LCOM].ShouldBe(0.667);
            handles.Metrics[CaDETMetric.CLOC].ShouldBe(100);
            handles.Metrics[CaDETMetric.NAD].ShouldBe(3);
            handles.Metrics[CaDETMetric.NMD].ShouldBe(4);
            handles.Metrics[CaDETMetric.WMC].ShouldBe(10);
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
            CodeModelFactory factory = new CodeModelFactory();
            var project = factory.CreateProjectWithCodeFileLinks(folderLocation);

            var classes = project.Classes.Select(c => c.FullName);
            var members = project.Classes.SelectMany(c => c.Members).Select(m => m.Signature());
            var keys = project.CodeLinks.Keys.ToList();
            var difference = classes.Union(members).Except(keys);

            difference.ShouldBeEmpty();
        }

        [Fact]
        public void Create_code_model_with_links_with_partial_classes()
        {
            CodeModelFactory factory = new CodeModelFactory(true);
            var project = factory.CreateProjectWithCodeFileLinks("C:/sdataset-partial");

            var projectClassesAndMembersCount = project.Classes.Count + project.Classes.Sum(c => c.Members.Count);

            project.CodeLinks.Count.ShouldBe(projectClassesAndMembersCount);
        }

        [Fact]
        public void Create_code_model_with_links_on_large_repo()
        {
            CodeModelFactory factory = new CodeModelFactory();
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

        [Theory]
        //[InlineData("C:\\temp\\challenge-sandbox\\start\\01. Structural Cohesion")]
        //[InlineData("C:\\temp\\challenge-sandbox\\end\\01. structural")]
        //[InlineData("C:\\temp\\challenge-sandbox\\start\\03. Coupling\\Employees")]
        //[InlineData("C:\\temp\\challenge-sandbox\\end\\03. coupling employee")]
        //[InlineData("C:\\temp\\challenge-sandbox\\start\\03. Coupling\\Rental")]
        //[InlineData("C:\\temp\\challenge-sandbox\\end\\03. coupling rental")]
        //[InlineData("C:\\temp\\challenge-sandbox\\start\\04. SRP\\Achievements")]
        //[InlineData("C:\\temp\\challenge-sandbox\\end\\04. srp achievements")]
        [InlineData("C:\\temp\\challenge-sandbox\\start\\04. SRP\\Books")]
        [InlineData("C:\\temp\\challenge-sandbox\\end\\04. srp books")]
        public void Create_code_model(string folderLocation)
        {
            CodeModelFactory factory = new CodeModelFactory();
            var project = factory.CreateProjectWithCodeFileLinks(folderLocation);

            var classes = project.Classes;
            var members = project.Classes.SelectMany(c => c.Members);

            var firstClassMetrics = classes.First().Metrics;
        }
    }
}
