using System;
using SmellDetector.Controllers;
using SmellDetector.Services;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;
using System.Linq;
using Xunit;
using Shouldly;
using SmellDetector.Communication;
using SmellDetectorTests.DataFactory;
using System.Collections.Generic;

namespace SmellDetectorTests.Unit
{
    public class SmellDetectorServiceTest
    {

        [Fact]
        public void Generate_Smell_Detection_Report_For_LongMethod_And_Long_Parameter_List_Issues()
        {
            DetectionService detectionService = new DetectionService();
            CaDETClassDTOFactory classFactory = new CaDETClassDTOFactory();

            classFactory.CreateIssuesLongMethodAndLongParameterList();
            var report = detectionService.GenerateSmellDetectionReport(classFactory.CaDETClassDTO);
            report.Report[classFactory.TestIdentifier].Count().ShouldBe(classFactory.ExpectedIssues);
        }

        [Fact]
        public void Generate_Smell_Detection_Report_For_LongMethod_Issue()
        {
            DetectionService detectionService = new DetectionService();
            CaDETClassDTOFactory classFactory = new CaDETClassDTOFactory();

            classFactory.CreateIssueLongMethod();
            var report = detectionService.GenerateSmellDetectionReport(classFactory.CaDETClassDTO);
            report.Report[classFactory.TestIdentifier].Count().ShouldBe(classFactory.ExpectedIssues);
        }

        [Fact]
        public void Generate_Smell_Detection_Report_For_Another_LongMethod_Issue()
        {
            DetectionService detectionService = new DetectionService();
            CaDETClassDTOFactory classFactory = new CaDETClassDTOFactory();

            classFactory.CreateAnotherIssueLongMethod();
            var report = detectionService.GenerateSmellDetectionReport(classFactory.CaDETClassDTO);
            report.Report[classFactory.TestIdentifier].Count().ShouldBe(classFactory.ExpectedIssues);

        [Theory]
        [MemberData(nameof(Data))]
        public void Generate_Smell_Detection_Report(String testIdentifier, MetricsDTO metricsForIdentifier, int expectedIssues)
        {
            DetectionService detectionService = new DetectionService();
            CaDETClassDTO caDetClassDto = new CaDETClassDTO();
            caDetClassDto.CodeItemMetrics[testIdentifier] = metricsForIdentifier;
            var report = detectionService.GenerateSmellDetectionReport(caDetClassDto);

            if (expectedIssues != 0){
                report.Report[testIdentifier].Count().ShouldBe(expectedIssues);
            }
            else{
                report.Report.Count().ShouldBe(expectedIssues);
            }

        }

        public static IEnumerable<object[]> Data()
        {
            List<object[]> retVal = new List<object[]>();

            retVal.Add(new object[] { "public void testMethod(int paramOne, int paramTwo, int another_paramTwo, int paramtwo, int paramTwos, int paramTwoas);", new MetricsDTO { NOP = 6 }, 2 });// one issue for long method(nop > 4) & one for long parameter list (nop > 5)
            retVal.Add(new object[] { "public void testMethod(int paramOne, int paramTwo, int another_paramTwo, int paramtwo, int moreparam);", new MetricsDTO { NOP = 5 }, 1 });// one issue for long method(nop > 4)
            retVal.Add(new object[] { "public void testMethod();", new MetricsDTO { NOP = 0, LOC = 100 }, 1 });// one for long method(loc > 50) 
            retVal.Add(new object[] { "public void testMethod2();", new MetricsDTO { NOP = 0 }, 0 });// there is no need for issues 
            retVal.Add(new object[] { "public class DoctorService", new MetricsDTO { WMC = 48, ATFD = 6, TCC = 0.32 }, 1});
            retVal.Add(new object[] { "public class DoctorService2", new MetricsDTO { LOC = 110 },  2}); // One issue for GodClass and one issue for LongMethod 
            retVal.Add(new object[] { "public class DoctorService3", new MetricsDTO { NMD = 11, NAD = 14 }, 1});
            
            return retVal;
        }
    }
}
