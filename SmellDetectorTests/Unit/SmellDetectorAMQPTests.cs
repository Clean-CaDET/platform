using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using SmellDetector.Communication;
using SmellDetector.SmellModel.Reports;
using SmellDetectorTests.DataFactory;
using Xunit;

namespace SmellDetectorTests.Unit
{
    public class SmellDetectorAMQPTests
    {
        private readonly ReportFactory _reportFactory = new ReportFactory();

        [Fact]
        public void Produce_Issue_Report_Message()
        {
            SmellDetectorMessageProducer producer = new SmellDetectorMessageProducer();
            SmellDetectionReport reportMessage = _reportFactory.CreateMockupReportMessage();
            producer.CreateNewIssueReport(reportMessage);
        }

        [Fact]
        public void Consume_Metrics_Report_Message()
        {
            SmellDetectorMessageConsumer consumer = new SmellDetectorMessageConsumer();
        }

    }
}
