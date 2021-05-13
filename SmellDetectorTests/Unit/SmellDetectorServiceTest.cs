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
        public void Generate_Smell_Detection_Report(List<CaDETClass> classes, int expectedIssues)
        {
            var detectionService = new DetectionService();
            var report = detectionService.GenerateSmellDetectionReport(classes);
            report.Report.Count().ShouldBe(expectedIssues);
        }

        public static IEnumerable<object[]> Data()
        {
            List<object[]> retVal = new List<object[]>();

            retVal.Add(new object[] {
                new List<CaDETClass> {
                    new CaDETClass
                    {
                        FullName = "Class1",
                        Metrics = new Dictionary<CaDETMetric, double> { { CaDETMetric.ATFD, 6 },
                            { CaDETMetric.WMC, 48 },
                            { CaDETMetric.TCC, 0.32},
                            { CaDETMetric.NMD, 0},
                            { CaDETMetric.NAD, 0},
                            { CaDETMetric.CLOC, 0},
                            { CaDETMetric.NMD_NAD, 0}}
                    },
                    new CaDETClass
                    {
                        FullName = "Class2",
                        Metrics = new Dictionary<CaDETMetric, double> { { CaDETMetric.ATFD, 0 },
                            { CaDETMetric.WMC, 0 },
                            { CaDETMetric.TCC, 0},
                            { CaDETMetric.NMD, 14},
                            { CaDETMetric.NAD, 16},
                            { CaDETMetric.CLOC, 0},
                            { CaDETMetric.NMD_NAD, 0}

                        }
                    },
                    new CaDETClass
                    {
                        FullName = "Class3",
                        Metrics = new Dictionary<CaDETMetric, double> { { CaDETMetric.ATFD, 0 },
                            { CaDETMetric.WMC, 0 },
                            { CaDETMetric.TCC, 0},
                            { CaDETMetric.NMD, 0},
                            { CaDETMetric.NAD, 0},
                            { CaDETMetric.CLOC, 120},
                            { CaDETMetric.NMD_NAD, 24} }
                    },
                    new CaDETClass
                    {
                        FullName = "Class4",
                        Metrics = new Dictionary<CaDETMetric, double> { { CaDETMetric.ATFD, 0 },
                            { CaDETMetric.WMC, 0 },
                            { CaDETMetric.TCC, 0},
                            { CaDETMetric.NMD, 0},
                            { CaDETMetric.NAD, 0},
                            { CaDETMetric.CLOC, 100},
                            { CaDETMetric.NMD_NAD, 0} }
                    }
                },
                3
            });
            return retVal;
        }
    }
}
