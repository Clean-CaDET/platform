using RepositoryCompiler.Communication;

namespace Platform.DataFactories
{
    public class MetricsReportFactory
    {
        public CaDETClassDTO CreateMockupMetricsReportMessage()
        {
            CaDETClassDTO report = new CaDETClassDTO();

            string exampleMethodId = "public void SavePerson(Person person);";

            MetricsDTO metrics = new MetricsDTO();
            metrics.LOC = 1000;
            metrics.NOLV = 100;
            metrics.NOP = 1;

            report.CodeItemMetrics[exampleMethodId] = metrics;

            return report;
        }
    }
}
