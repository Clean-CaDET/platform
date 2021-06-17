using CodeModel.CaDETModel.CodeItems;
using Shouldly;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace CodeModel.Tests.Unit
{
    public class ICMBCCalculationTests
    {
        [Theory]
        [MemberData(nameof(GetICMBCClasses))]
        public void Test_ICMBC_cohesion_metric(string classPath, double expectedValue)
        {
            string[] classCode = GetCode(classPath);
            var project = new CodeModelFactory().CreateProject(classCode);

            var actualValue = project.Classes[0].Metrics[CaDETMetric.ICBMC];

            actualValue.ShouldBe(expectedValue);
        }

        public static IEnumerable<object[]> GetICMBCClasses() => new List<object[]>()
        {
            new object[]
            {
                @"CohesionClasses\HighestCohesion.txt",
                1
            },
            new object[]
            {
                @"CohesionClasses\LowestCohesion.txt",
                0
            },
            new object[]
            {
                @"CohesionClasses\TestClass01.txt",
                0.25
            },
            new object[]
            {
                @"CohesionClasses\TestClass02.txt",
                0.33
            },
            new object[]
            {
                @"CohesionClasses\TestClass1.txt",
                0.16
            },
            new object[]
            {
                @"CohesionClasses\TestClass2.txt",
                0.33
            },
            new object[]
            {
                @"CohesionClasses\TestClass3.txt",
                0.42
            },
            new object[]
            {
                @"CohesionClasses\TestClass4.txt",
                0.05
            },
            new object[]
            {
                @"CohesionClasses\TestClass5.txt",
                0.1
            },
            new object[]
            {
                @"CohesionClasses\TestClass6.txt",
                0.33
            },
            new object[]
            {
                @"CohesionClasses\TestClass7.txt",
                0.22
            },
            new object[]
            {
                @"CohesionClasses\TestClass8.txt",
                1
            }
        };

        private static string[] GetCode(string projectPath)
        {
            string classCode = File.ReadAllText("../../../TestData/" + projectPath);
            return new[] {classCode};
    }
}

}