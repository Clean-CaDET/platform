using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RepositoryCompiler.Communication
{
    /// <summary>
    /// This is pure POC for projects communication.
    /// 
    /// We need to refactor and make more suitable infrastructure !
    /// </summary>
    public class RepositoryCompilerMessageProducer
    {
        public string NodeName { get; set; }
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public IConnection Connection { get; set; }
        public IModel Channel { get; set; }

        public RepositoryCompilerMessageProducer()
        {
            ConfigureInitialStates();
            CreateConnection();
            Channel = Connection.CreateModel();
            DeclareQueue();
        }

        public void CreateNewMetricsReport(string reportMessage)
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
            QueueName = "MetricsReports";
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

        private byte[] GetEncodedMessage(string reportMessage)
        {
            return Encoding.UTF8.GetBytes(reportMessage);
        }
    }
}
