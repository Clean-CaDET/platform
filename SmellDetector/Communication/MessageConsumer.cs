using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SmellDetector.Services;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;

namespace SmellDetector.Communication
{
    public class MessageConsumer
    {
        public string NodeName { get; set; }
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public IConnection Connection { get; set; }

        public List<IModel> Channel { get; set; }
        public MessageConsumer()
        {
            ConfigureInitialStates();
            CreateConnection();
            Channel = new List<IModel>();
            for (int numberOfChannelsPerConnection = 0; numberOfChannelsPerConnection < 5; numberOfChannelsPerConnection++)
            {
                Channel.Add(Connection.CreateModel());
            }
            DeclareQueue();

            ConsumeMessage();
        }

        private void ConfigureInitialStates()
        {
            NodeName = "localhost";
            QueueName = "MetricsReports";
            ExchangeName = "";
        }

        private void CreateConnection()
        {
            var connectionFactory = new ConnectionFactory { HostName = NodeName, RequestedChannelMax = 10 };
            Connection = connectionFactory.CreateConnection();
        }

        private void DeclareQueue()
        {
            foreach (IModel channel in Channel)
            {
                channel.QueueDeclare(queue: QueueName,
                                                 durable: false,
                                                 exclusive: false,
                                                 autoDelete: false,
                                                 arguments: null);
            }
        }

        private void ConsumeMessage()
        {

            foreach (IModel channel in Channel)
            {
                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, deliveryArguments) =>
            {
                var body = deliveryArguments.Body.ToArray();
                var jsonMessage = Encoding.UTF8.GetString(body);
                CaDETClassDTO repositoryCompilerReport = new CaDETClassDTO();
                try
                {
                    repositoryCompilerReport = JsonConvert.DeserializeObject<CaDETClassDTO>(jsonMessage);
                    SendIssueReportToSmartTutor(ProcessRepositoryCompilerReport(repositoryCompilerReport));
                }
                catch (Exception)
                {
                    //TODO: write exc
                }
            };

                channel.BasicConsume(queue: QueueName,
                                                     autoAck: true,
                                                     consumer: consumer);
            }
        }

        private void SendIssueReportToSmartTutor(SmellDetectionReport smellDetectionReport)
        {
            ApplicationBuilderExtentions._producer.CreateNewIssueReport(smellDetectionReport);
        }

        private SmellDetectionReport ProcessRepositoryCompilerReport(CaDETClassDTO repositoryCompilerReport)
        {
            DetectionService detectionService = new DetectionService();
            return detectionService.GenerateSmellDetectionReport(repositoryCompilerReport);
        }

    }
}
