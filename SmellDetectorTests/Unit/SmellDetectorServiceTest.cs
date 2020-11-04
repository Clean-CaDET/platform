using System;
using SmellDetector.Controllers;
using SmellDetector.Services;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;
using System.Linq;
using Xunit;
using Shouldly;

namespace SmellDetectorTests.Unit
{
    public class SmellDetectorServiceTest
    {

        [Fact]
        public void Generate_Smell_Detection_Report_For_LongMethod_And_Long_Parameter_List_Issues()
        {
            DetectionService detectionService = new DetectionService();

            CaDETClassDTO caDetClassDto = new CaDETClassDTO();
            string testIdentifier = "public void testMethod(int paramOne, int paramTwo, int another_paramTwo, int paramtwo, int paramTwos, int paramTwoas);";
            MetricsDTO metricsForIdentifier = new MetricsDTO();
            metricsForIdentifier.NOP = 6;
            caDetClassDto.CodeItemMetrics[testIdentifier] = metricsForIdentifier;
            var expectedIssues = 2; // one for long method(nop > 4) & one for long parameter list (nop > 5)

            var report = detectionService.GenerateSmellDetectionReport(caDetClassDto);
            report.Report[testIdentifier].Count().ShouldBe(expectedIssues);
        }

        [Fact]
        public void Generate_Smell_Detection_Report_For_LongMethod_Issue()
        {
            DetectionService detectionService = new DetectionService();

            CaDETClassDTO caDetClassDto = new CaDETClassDTO();
            string testIdentifier = "public void testMethod(int paramOne, int paramTwo, int another_paramTwo, int paramtwo, int moreparam);";
            MetricsDTO metricsForIdentifier = new MetricsDTO();
            metricsForIdentifier.NOP = 5;
            caDetClassDto.CodeItemMetrics[testIdentifier] = metricsForIdentifier;
            var expectedIssues = 1; // one for long method(nop > 4) 

            var report = detectionService.GenerateSmellDetectionReport(caDetClassDto);
            report.Report[testIdentifier].Count().ShouldBe(expectedIssues);

            caDetClassDto = new CaDETClassDTO();
            testIdentifier = "public void testMethod();";
            metricsForIdentifier = new MetricsDTO();
            metricsForIdentifier.NOP = 0;
            metricsForIdentifier.LOC = 100;
            caDetClassDto.CodeItemMetrics[testIdentifier] = metricsForIdentifier;
            expectedIssues = 1; // one for long method(loc > 50) 

            report = detectionService.GenerateSmellDetectionReport(caDetClassDto);
            report.Report[testIdentifier].Count().ShouldBe(expectedIssues);
        }

        [Fact]
        public void Generate_Smell_Detection_Report_Without_Issues()
        {
            DetectionService detectionService = new DetectionService();

            CaDETClassDTO caDetClassDto = new CaDETClassDTO();
            string testIdentifier = "public void testMethod();";
            MetricsDTO metricsForIdentifier = new MetricsDTO();
            metricsForIdentifier.NOP = 0;
            caDetClassDto.CodeItemMetrics[testIdentifier] = metricsForIdentifier;
            var expectedIssues = 0; // there is no need for issues

            var report = detectionService.GenerateSmellDetectionReport(caDetClassDto);
            report.Report.Count().ShouldBe(expectedIssues);
        }

    }
}
