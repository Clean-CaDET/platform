using System;
using RabbitMQ.Client;
using System.Text;

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
            string message = "Hello World!";
            var encodedMessage = Encoding.UTF8.GetBytes(message);
            return encodedMessage;
        }
    }
}
