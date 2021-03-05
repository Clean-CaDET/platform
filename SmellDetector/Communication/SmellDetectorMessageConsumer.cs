using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SmellDetector.SmellModel;
using SmellDetector.SmellModel.Reports;

namespace SmellDetector.Communication
{
    public class SmellDetectorMessageConsumer
    {
        public string NodeName { get; set; }
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public IConnection Connection { get; set; }
        public IModel Channel { get; set; }
        public SmellDetectorMessageConsumer()
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
            QueueName = "MetricsReports";
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
                CaDETClassDTO reportMessage = new CaDETClassDTO();
                try
                {
                    reportMessage = JsonConvert.DeserializeObject<CaDETClassDTO>(jsonMessage);
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
