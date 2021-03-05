using RepositoryCompiler.Communication;

namespace RepositoryCompilerTests.DataFactories
{
    public class MetricsReportFactory
    {
        public CaDETClassDTO CreateMockupMetricsReportMessage()
        {
            CaDETClassDTO report = new CaDETClassDTO();

            string exampleClassId = "public class Person";

            MetricsDTO metrics = new MetricsDTO();
            metrics.LOC = 1000;
            metrics.NOLV = 100;

            report.CodeItemMetrics[exampleClassId] = metrics;

            return report;
        }
    }
}
