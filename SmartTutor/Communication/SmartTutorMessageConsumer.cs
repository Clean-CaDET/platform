using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace SmartTutor.Communucation
{

    public class SmartTutorMessageConsumer
    {
        public string NodeName { get; set; }
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public IConnection Connection { get; set; }
        public IModel Channel { get; set; }

        public SmartTutorMessageConsumer()
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
                var message = Encoding.UTF8.GetString(body);

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
