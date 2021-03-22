using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RepositoryCompiler.Communication
{
    /// <summary>
    /// This is pure POC for projects communication.
    /// 
    /// We need to refactor and make more suitable infrastructure !
    /// </summary>
    public class MessageProducer
    {
        public string NodeName { get; set; }
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public IConnection Connection { get; set; }
        public IModel Channel { get; set; }

        public MessageProducer()
        {
            ConfigureInitialStates();
            CreateConnection();
            Channel = Connection.CreateModel();
            DeclareQueue();
        }

        public void CreateNewMetricsReport(CaDETClassDTO reportMessage)
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

        private byte[] GetEncodedMessage(CaDETClassDTO reportMessage)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reportMessage));
        }
    }
}
