using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.CaDETModel.CodeItems;
using Shouldly;
using Xunit;

namespace CodeModel.Tests.Unit
{
    public class ICMBCCalculationTests
    {
        [Theory]
        [MemberData(nameof(GetICMBCClasses))]
        public void Test_ICMBC_cohesion_metric(string projectPath, List<double> expectedValue)
        {
            string[] sourceCodes = GetCode(projectPath);
            var project = new CodeModelFactory().CreateProject(sourceCodes);

            var actualValues = project.Classes.Select(c => c.Metrics[CaDETMetric.ICBMC]);

            actualValues.Count().ShouldBe(expectedValue.Count);
            actualValues.All(expectedValue.Contains).ShouldBeTrue();
        }

        public static IEnumerable<object[]> GetICMBCClasses() => new List<object[]>()
        {
            new object[]
            {
                "CohesionClasses",
                new List<double>
                {
                    1,
                    0,
                    0.25,
                    0.33,
                    0.16,
                    0.33,
                    0.42,
                    0.05,
                    0.1,
                    0.33,
                    0.22,
                    1
                }
            }
        };

        private static string[] GetCode(string projectPath)
        {
            var testDataFiles = Directory.GetFiles("../../../TestData/" + projectPath);
            return testDataFiles.Select(File.ReadAllText).ToArray();
        }
    }
}