using System;
using RabbitMQ.Client;
using System.Text;
using SmellDetector.SmellModel.Reports;
using System.Collections.Generic;
using SmellDetector.SmellModel;
using Newtonsoft.Json;

namespace SmellDetector.Communication
{
    /// <summary>
    /// This is pure POC for projects communication.
    /// 
    /// We need to refactor and make more suitable infrastructure !
    /// </summary>
    public class SmellDetectorMessageProducer
    {
        public string NodeName { get; set; }
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public IConnection Connection { get; set; }
        public IModel Channel { get; set; }

        public SmellDetectorMessageProducer()
        {
            ConfigureInitialStates();
            CreateConnection();
            Channel = Connection.CreateModel();
            DeclareQueue();
        }

        public void CreateNewIssueReport(SmellDetectionReport reportMessage)
        {
            PublishMessage(GetEncodedMessage(reportMessage));
        }

        private void CreateConnection()
        {
            var connectionFactory = new ConnectionFactory() { HostName = NodeName };
            Connection = connectionFactory.CreateConnection();
        }

        private void ConfigureInitialStates()
        {
            NodeName = "localhost";
            QueueName = "IssueReports";
            ExchangeName = "";
        }

        private void PublishMessage(byte[] body)
        {
            Channel.BasicPublish(exchange: ExchangeName,
                                                 routingKey: QueueName,
                                                 basicProperties: null,
                                                 body: body);
        }

        private void DeclareQueue()
        {
            Channel.QueueDeclare(queue: QueueName,
                                                 durable: false,
                                                 exclusive: false,
                                                 autoDelete: false,
                                                 arguments: null);
        }

        private byte[] GetEncodedMessage(SmellDetectionReport reportMessage)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reportMessage));
        }
    }
}
