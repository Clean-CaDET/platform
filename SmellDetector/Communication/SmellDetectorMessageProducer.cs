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
        public SmellDetectorMessageProducer()
        {

            var nodeName = "localhost";
            var queueName = "IssueReports";
            var exchangeName = "";

            var connectionFactory = new ConnectionFactory() { HostName = nodeName };
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                DeclareQueue(queueName, channel);
                PublishMessage(queueName, exchangeName, channel, EncodeMessage());
            }
        }

        private void PublishMessage(string queueName, string exchangeName, IModel channel, byte[] body)
        {
            channel.BasicPublish(exchange: exchangeName,
                                                 routingKey: queueName,
                                                 basicProperties: null,
                                                 body: body);
        }

        private void DeclareQueue(string queueName, IModel channel)
        {
            channel.QueueDeclare(queue: queueName,
                                                 durable: false,
                                                 exclusive: false,
                                                 autoDelete: false,
                                                 arguments: null);
        }

        private byte[] EncodeMessage()
        {
            SmellDetectionReport reportMessage = new SmellDetectionReport();
            reportMessage.Report = new Dictionary<string, List<Issue>>();

            Issue detectedIssue = new Issue();
            detectedIssue.IssueType = SmellType.GOD_CLASS;
            detectedIssue.CodeItemId = "public class Doctor";

            List<Issue> detectedIssues = new List<Issue>();
            detectedIssues.Add(detectedIssue);

            reportMessage.Report.Add("Identifikator", detectedIssues);

            var encodedMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reportMessage));

            return encodedMessage;
        }
    }
}
