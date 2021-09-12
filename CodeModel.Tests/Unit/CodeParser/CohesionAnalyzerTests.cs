using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeModel.CaDETModel.CodeItems;
using CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer;
using CodeModel.CodeParsers.CSharp.Exceptions;
using Xunit;

namespace CodeModel.Tests.Unit.CodeParser
{
    public class CohesionAnalyzerTests
    {
        [Fact]
        public void Test_Class_Without_Data_Members()
        {
            string[] classCode = GetCode("CohesionAnalyzer/ClassWithoutDataMembers.txt");
            CaDETClass testClass = new CodeModelFactory().CreateProject(classCode).Classes[0];

            var analyzer = new CohesionAnalyzer(new Coh());
            Exception ex = Assert.Throws<ClassWithoutElementsException>(() =>
                analyzer.IdentifyCohesiveParts(testClass));
            Assert.Equal("Class `CohesionAnalyzer` has no data members.", ex.Message);
        }

        [Fact]
        public void Test_Class_Without_Normal_Methods()
        {
            string[] classCode = GetCode("CohesionAnalyzer/ClassWithoutMethods.txt");
            CaDETClass testClass = new CodeModelFactory().CreateProject(classCode).Classes[0];

            var analyzer = new CohesionAnalyzer(new Coh());
            Exception ex = Assert.Throws<ClassWithoutElementsException>(() =>
                analyzer.IdentifyCohesiveParts(testClass));
            Assert.Equal("Class `CohesionAnalyzer` has no normal methods.", ex.Message);
        }

        [Theory]
        [MemberData(nameof(GetTestClassesWithSingleResult))]
        public void Test_Identify_Cohesive_Parts_Within_A_Class(string classPath, int resultsCount,
            int[] accessesToCutCounts, int[] firstPartsCounts, int[] secondPartsCounts)
        {
            string[] classCode = GetCode(classPath);
            CaDETClass testClass = new CodeModelFactory().CreateProject(classCode).Classes[0];

            CohesionAnalyzer analyzer = new CohesionAnalyzer(new Coh());
            var result = analyzer.IdentifyCohesiveParts(testClass);
            Assert.Equal(resultsCount, result.Count);
            Assert.Equal(accessesToCutCounts, result.Select(res => res.AccessesToRemove.Count));
            Assert.Equal(firstPartsCounts, result.Select(res => res.Parts[0].Accesses.Count));
            Assert.Equal(secondPartsCounts, result.Select(res => res.Parts[1].Accesses.Count));
        }

        public static IEnumerable<object[]> GetTestClassesWithSingleResult() => new List<object[]>
        {
            new object[]
            {
                @"CohesionAnalyzer/ClassInteractions.txt",
                1,
                new[] { 1 },
                new[] { 1 },
                new[] { 1 }
            },
            new object[]
            {
                @"CohesionAnalyzer/DisconnectedClass.txt",
                1,
                new[] { 0 },
                new[] { 1 },
                new[] { 4 }
            },
            new object[]
            {
                @"CohesionAnalyzer/SurfPhysics.txt",
                1,
                new[] { 1 },
                new[] { 3 },
                new[] { 4 }
            },
            new object[]
            {
                @"CohesionAnalyzer/UserController.txt",
                2,
                new[] { 4, 5 },
                new[] { 8, 5 },
                new[] { 4, 6 }
            },
            new object[]
            {
                @"CohesionAnalyzer/FullyConnectedClass.txt",
                0,
                Array.Empty<object>(),
                Array.Empty<object>(),
                Array.Empty<object>()
            },
            new object[]
            {
                @"CohesionAnalyzer/HardLinkHelper.txt",
                1,
                new[] { 0 },
                new[] { 5 },
                new[] { 1 }
            }
        };

        private string[] GetCode(string path)
        {
            string classCode = File.ReadAllText("../../../DataFactories/TestClasses/" + path);
            return new[] { classCode };
        }
    }
}