using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SmellDetector.SmellModel.Reports;
using System;
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
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (model, deliveryArguments) =>
            {
                var body = deliveryArguments.Body.ToArray();
                var jsonMessage = Encoding.UTF8.GetString(body);
                SmellDetectionReport reportMessage = new SmellDetectionReport();
                try
                {   
                    reportMessage = JsonConvert.DeserializeObject<SmellDetectionReport>(jsonMessage);
                }
                catch (Exception)  
                {
                   //TODO: write exc
                }
            };
            return consumer;
        }

        private void ConsumeMessage(EventingBasicConsumer consumer)
        {
            Channel.BasicConsume(queue: QueueName,
                                                 autoAck: true,
                                                 consumer: consumer);
        }
    }
}
