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
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: exchangeName,
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
