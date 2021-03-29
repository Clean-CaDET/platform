using System;
using SmellDetector.Controllers;
using SmellDetector.Services;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;
using System.Linq;
using Xunit;
using Shouldly;
using System.Collections.Generic;

namespace SmellDetectorTests.Unit
{
    public class SmellDetectorServiceTest
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void Generate_Smell_Detection_Report(List<CaDETClassDTO> caDETClassDTOs, int expectedIssues)
        {
            DetectionService detectionService = new DetectionService();
            List<CaDETClassDTO> caDETClassDTOList = caDETClassDTOs;
            var report = detectionService.GenerateSmellDetectionReport(caDETClassDTOList);
            report.Report.Count().ShouldBe(expectedIssues);
        }

        public static IEnumerable<object[]> Data()
        {
            List<object[]> retVal = new List<object[]>();

            retVal.Add(new object[] {
                new List<CaDETClassDTO> {
                    new CaDETClassDTO("public class DoctorService",
                                      new Dictionary<string, double> { { "ATFD", 6 }, { "WMC", 48 }, { "TCC", 0.32 } }),
                    new CaDETClassDTO("public class DoctorService2",
                                      new Dictionary<string, double> { { "NMD", 16 }, { "NAD", 14 } }),
                    new CaDETClassDTO("public class DoctorService3",
                                      new Dictionary<string, double> { { "LOC", 120 }, { "[NOM] + [NOF]", 24 } }),
                    new CaDETClassDTO("public class DoctorService4",
                                      new Dictionary<string, double> { { "LOC", 100 } })}, 3 });
            return retVal;
        }

        //public static IEnumerable<object[]> Data()
        //{
        //    List<object[]> retVal = new List<object[]>();

        //    retVal.Add(new object[] { "public void testMethod(int paramOne, int paramTwo, int another_paramTwo, int paramtwo, int paramTwos, int paramTwoas);", new MetricsDTO { NOP = 6 }, 2 });// one issue for long method(nop > 4) & one for long parameter list (nop > 5)
        //    retVal.Add(new object[] { "public void testMethod(int paramOne, int paramTwo, int another_paramTwo, int paramtwo, int moreparam);", new MetricsDTO { NOP = 5 }, 1 });// one issue for long method(nop > 4)
        //    retVal.Add(new object[] { "public void testMethod();", new MetricsDTO { NOP = 0, LOC = 100 }, 1 });// one for long method(loc > 50) 
        //    retVal.Add(new object[] { "public void testMethod2();", new MetricsDTO { NOP = 0 }, 0 });// there is no need for issues 
        //    retVal.Add(new object[] { "public class DoctorService", new MetricsDTO { WMC = 48, ATFD = 6, TCC = 0.32 }, 1});
        //    retVal.Add(new object[] { "public class DoctorService2", new MetricsDTO { LOC = 110 },  1}); // One issue for LongMethod 
        //    retVal.Add(new object[] { "public class DoctorService3", new MetricsDTO { NMD = 16, NAD = 14 }, 1});

        //    return retVal;
        //}

    }
}
