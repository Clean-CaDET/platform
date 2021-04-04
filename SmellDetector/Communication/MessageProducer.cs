using System;
using System.Collections.Concurrent;
using RabbitMQ.Client;
using System.Text;
using SmellDetector.SmellModel.Reports;
using System.Collections.Generic;
using System.Linq;
using SmellDetector.SmellModel;
using Newtonsoft.Json;

namespace SmellDetector.Communication
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
        public ConcurrentDictionary<ulong, byte[]> OutstandingConfirms { get; set; }

        public MessageProducer()
        {
            ConfigureInitialStates();
            CreateConnection();
            CreateChannels(5);
            DeclareQueue();
        }

        private void ConfigureInitialStates()
        {
            NodeName = "localhost";
            QueueName = "IssueReports";
            ExchangeName = "";

            Channel = new List<IModel>();
            OutstandingConfirms = new ConcurrentDictionary<ulong, byte[]>();
        }

        private void CreateConnection()
        {
            var connectionFactory = new ConnectionFactory { HostName = NodeName, RequestedChannelMax = 10 };
            Connection = connectionFactory.CreateConnection();
        }

        private void CreateChannels(int numberOfChannelsPerConnection)
        {
            for (var i = 0; i < numberOfChannelsPerConnection; i++)
            {
                var channel = Connection.CreateModel();
                channel.ConfirmSelect();
                ConfigurePublisherConfirms(channel);
                Channel.Add(channel);
            }
        }

        private void ConfigurePublisherConfirms(IModel channel)
        {
            channel.BasicAcks += (sender, ea) =>
            {
                CleanOutstandingConfirms(ea.DeliveryTag, ea.Multiple);
            };
            channel.BasicNacks += (sender, ea) =>
            {
                OutstandingConfirms.TryGetValue(ea.DeliveryTag, out byte[] body);
                // TODO: Make some handling for nacked messages
                Console.WriteLine($"\n\nMessage with body {body} has been nack-ed. Sequence number: {ea.DeliveryTag}, multiple: {ea.Multiple} \n\n");
                CleanOutstandingConfirms(ea.DeliveryTag, ea.Multiple);
            };
        }

        private void CleanOutstandingConfirms(ulong sequenceNumber, bool multiple)
        {
            if (multiple)
            {
                var confirmed = OutstandingConfirms.Where(k => k.Key <= sequenceNumber);
                foreach (var entry in confirmed)
                {
                    OutstandingConfirms.TryRemove(entry.Key, out _);
                }
            }
            else
            {
                OutstandingConfirms.TryRemove(sequenceNumber, out _);
            }
        }

        public void CreateNewIssueReport(SmellDetectionReport reportMessage)
        {
            PublishMessage(GetEncodedMessage(reportMessage));
        }

        private void DeclareQueue()
        {
            foreach (var channel in Channel)
            {
                channel.QueueDeclare(queue: QueueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
            }
        }

        private void PublishMessage(byte[] body)
        {
            var random = new Random();
            var randomChannelIndex = random.Next(Channel.Count);
            var randomAvailableChannel = Channel[randomChannelIndex];

            OutstandingConfirms.TryAdd(randomAvailableChannel.NextPublishSeqNo, body);
            randomAvailableChannel.BasicPublish(exchange: ExchangeName,
                                                 routingKey: QueueName,
                                                 basicProperties: null,
                                                 body: body);
        }

        private byte[] GetEncodedMessage(SmellDetectionReport reportMessage)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reportMessage));
        }
    }
}
