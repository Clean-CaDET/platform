using System;
using SmellDetector.Controllers;
using SmellDetector.Services;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;
using System.Linq;
using Xunit;
using Shouldly;
using System.Collections.Generic;
using CodeModel.CaDETModel.CodeItems;

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
                                      new Dictionary<CaDETMetric, double> { { CaDETMetric.ATFD, 6 },
                                                                            { CaDETMetric.WMC, 48 },
                                                                            { CaDETMetric.TCC, 0.32},
                                                                            { CaDETMetric.NMD, 0},
                                                                            { CaDETMetric.NAD, 0},
                                                                            { CaDETMetric.CLOC, 0},
                                                                            { CaDETMetric.NMD_NAD, 0}}),
                    new CaDETClassDTO("public class DoctorService2",
                                      new Dictionary<CaDETMetric, double> { { CaDETMetric.ATFD, 0 },
                                                                            { CaDETMetric.WMC, 0 },
                                                                            { CaDETMetric.TCC, 0},
                                                                            { CaDETMetric.NMD, 14},
                                                                            { CaDETMetric.NAD, 16},
                                                                            { CaDETMetric.CLOC, 0},
                                                                            { CaDETMetric.NMD_NAD, 0} }),
                    new CaDETClassDTO("public class DoctorService3",
                                      new Dictionary<CaDETMetric, double> { { CaDETMetric.ATFD, 0 },
                                                                            { CaDETMetric.WMC, 0 },
                                                                            { CaDETMetric.TCC, 0},
                                                                            { CaDETMetric.NMD, 0},
                                                                            { CaDETMetric.NAD, 0},
                                                                            { CaDETMetric.CLOC, 120},
                                                                            { CaDETMetric.NMD_NAD, 24} }),
                    new CaDETClassDTO("public class DoctorService4",
                                      new Dictionary<CaDETMetric, double> { { CaDETMetric.ATFD, 0 },
                                                                            { CaDETMetric.WMC, 0 },
                                                                            { CaDETMetric.TCC, 0},
                                                                            { CaDETMetric.NMD, 0},
                                                                            { CaDETMetric.NAD, 0},
                                                                            { CaDETMetric.CLOC, 100},
                                                                            { CaDETMetric.NMD_NAD, 0} })}, 3 });
            return retVal;
        }
    }
}
