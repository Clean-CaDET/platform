using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace SmartTutor.Communucation
{

    public class SmartTutorMessageConsumer
    {
        public SmartTutorMessageConsumer()
        {
            var nodeName = "localhost";
            var queueName = "IssueReports";

            var connectionFactory = new ConnectionFactory() { HostName = nodeName };
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                DeclareQueue(queueName, channel);
                ConsumeMessage(queueName, channel, DecodeMessage(channel));
            }
        }

        private static EventingBasicConsumer DecodeMessage(IModel channel)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, deliveryArguments) =>
            {
                var body = deliveryArguments.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

            };
            return consumer;
        }

        private static void ConsumeMessage(string queueName, IModel channel, EventingBasicConsumer consumer)
        {
            channel.BasicConsume(queue: queueName,
                                                 autoAck: true,
                                                 consumer: consumer);
        }

        private static void DeclareQueue(string queueName, IModel channel)
        {
            channel.QueueDeclare(queue: queueName,
                                                 durable: false,
                                                 exclusive: false,
                                                 autoDelete: false,
                                                 arguments: null);
        }
    }
}
