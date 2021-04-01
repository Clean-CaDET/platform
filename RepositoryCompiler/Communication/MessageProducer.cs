using System;
using System.Collections.Generic;
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
        public List<IModel> Channel { get; set; }

        public MessageProducer()
        {
            ConfigureInitialStates();
            CreateConnection();
            Channel = new List<IModel>();
         
            for (int numberOfChannelsPerConnection = 0; numberOfChannelsPerConnection < 5; numberOfChannelsPerConnection++)
            {
                Channel.Add(Connection.CreateModel());
            }
         
            DeclareQueue();
        }

        public void CreateNewMetricsReport(CaDETClassDTO reportMessage)
        {
            PublishMessage(GetEncodedMessage(reportMessage));
        }

        private void CreateConnection()
        {
            var connectionFactory = new ConnectionFactory { HostName = NodeName, RequestedChannelMax = 10 };
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
            Random random = new Random();
            int randomChannelIndex = random.Next(Channel.Count);
            Channel[randomChannelIndex].BasicPublish(exchange: ExchangeName,
                routingKey: QueueName,
                basicProperties: null,
                body: body);
        }

        private void DeclareQueue()    
        {
            foreach(IModel channel in Channel){
                channel.QueueDeclare(queue: QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            }
           
        }

        private byte[] GetEncodedMessage(CaDETClassDTO reportMessage)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reportMessage));
        }
    }
}
