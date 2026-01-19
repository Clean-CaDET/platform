using CodeModel.CaDETModel.CodeItems;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmellDetector.Tests.Unit
{
    public class SmellDetectorServiceTest
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void Smell_detector_sanity_check(string[] sourceCode, int expectedIssues)
        {
            var detectionService = new SmellDetectorService();
            var report = detectionService.AnalyzeCodeQuality(sourceCode);
            report.IssuesForCodeSnippet.Count().ShouldBe(expectedIssues);
        }

        public static IEnumerable<object[]> Data()
        {
            List<object[]> testData = new List<object[]>();

            testData.Add(new object[] {
                new string[] { "" },
                0
            });
            return testData;
        }
    }
}
