using Shouldly;
using Platform.DataFactories;
using RepositoryCompiler.Communication;
using SmartTutor.Communication;
using SmellDetector.Communication;
using Xunit;

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
            var producer = new RepositoryCompiler.Communication.MessageProducer();
            var reportMessage = _metricsReportFactory.CreateMockupMetricsReportMessage();
            producer.CreateNewMetricsReport(reportMessage);
        }

        private void CreateIssueReport()
        {
            var consumer = new SmellDetector.Communication.MessageConsumer();
        }

        private void ReadIssueReport()
        {
            var consumer = new SmartTutor.Communication.MessageConsumer();
        }
    }
}