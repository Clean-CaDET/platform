using Shouldly;
using Platform.DataFactories;
using RepositoryCompiler.Communication;
using SmartTutor.Communucation;
using SmellDetector.Communication;
using Xunit;
using CaDETClassDTO =
    RepositoryCompiler.Communication.CaDETClassDTO; // TODO: Decide where to storage same files between services

namespace Platform.Integration
{
    // TODO: Find better place for this test class !
    public class PlatformAMQPTest
    {
        private readonly MetricsReportFactory _metricsReportFactory = new MetricsReportFactory();

        [Fact]
        public void Produce_Metrics_Report_Message()
        {
            CreateMetricsReport();
            CreateIssueReport();
            ReadIssueReport();
        }

        private void CreateMetricsReport()
        {
            RepositoryCompilerMessageProducer producer = new RepositoryCompilerMessageProducer();
            CaDETClassDTO reportMessage = _metricsReportFactory.CreateMockupMetricsReportMessage();
            producer.CreateNewMetricsReport(reportMessage);
        }

        private void CreateIssueReport()
        {
            SmellDetectorMessageConsumer consumer = new SmellDetectorMessageConsumer();
        }

        private void ReadIssueReport()
        {
            SmartTutorMessageConsumer consumer = new SmartTutorMessageConsumer();
        }
    }
}