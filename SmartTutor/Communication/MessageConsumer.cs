using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SmartTutor.ContentModel;
using SmartTutor.Repository;
using SmartTutor.Service;
using SmellDetector.SmellModel.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartTutor.Communication
{

    public class MessageConsumer
    {

        public string NodeName { get; set; }
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public IConnection Connection { get; set; }
        public IModel Channel { get; set; }

        public MessageConsumer()
        {
            ConfigureInitialStates();
            CreateConnection();
            Channel = Connection.CreateModel();
            DeclareQueue();
            ConsumeMessage(DecodeMessage());
        }

        private void ConfigureInitialStates()
        {
            NodeName = "localhost";
            QueueName = "IssueReports";
            ExchangeName = "";
        }

        private void CreateConnection()
        {
            var connectionFactory = new ConnectionFactory() { HostName = NodeName };
            Connection = connectionFactory.CreateConnection();
        }

        private void DeclareQueue()
        {
            Channel.QueueDeclare(queue: QueueName,
                                                 durable: false,
                                                 exclusive: false,
                                                 autoDelete: false,
                                                 arguments: null);
        }

        private EventingBasicConsumer DecodeMessage()
        {
            ReportMessagesClass reportMessagesClass = ReportMessagesClass.Instance;
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (model, deliveryArguments) =>
            {
                var body = deliveryArguments.Body.ToArray();
                var jsonMessage = Encoding.UTF8.GetString(body);
                SmellDetectionReport reportMessage = new SmellDetectionReport();
                try
                {   
                    reportMessage = JsonConvert.DeserializeObject<SmellDetectionReport>(jsonMessage);

                    // TODO: Decide how many content we need in summary

                    Random random = new Random();
                    int numberOfeducationContents = FindEducationContentForReportMessage(reportMessage).Count();
                    int randomEducatinContent = random.Next(numberOfeducationContents);

                    reportMessagesClass.ReportMessages[reportMessage.Id] = FindEducationContentForReportMessage(reportMessage)[randomEducatinContent]; 
                }
                catch (Exception)  
                {
                   //TODO: write exc
                }
            };
            return consumer;
        }

        private List<EducationContent> FindEducationContentForReportMessage(SmellDetectionReport reportMessage)
        {
            var contentService = new ContentService(new ContentInMemoryRepository());
            var educationContent = new List<EducationContent>();
            var allKeys = reportMessage.Report.Keys;

            foreach (var key in allKeys)
            {
                foreach (var issue in reportMessage.Report[key])
                {
                    // TODO: Decide how many content we need for each issue, there is only first only with 0
                    educationContent.Add(contentService.FindContentForIssue(issue.IssueType, 0));
                }
            }

            return educationContent;
        }

        private void ConsumeMessage(EventingBasicConsumer consumer)
        {
            Channel.BasicConsume(queue: QueueName,
                                                 autoAck: true,
                                                 consumer: consumer);
        }
    }
}
