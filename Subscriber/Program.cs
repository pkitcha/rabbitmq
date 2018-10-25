using System;
using EasyNetQ;
using Message;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;

namespace Subscriber
{
    class Program
    {


        static void Main(string[] args)
        {
            var userName = "consumer";
            var passWord = "consumer";
            var hostName = "localhost";
            var queqeName = "PPI-QUEUE";
            var exchangeName = "PPI-EXCHANGE";
            var routingKey = "PPI-ROUTING-KEY";

            var factory = new ConnectionFactory();
            factory.UserName = userName;
            factory.Password = passWord;
            factory.HostName = hostName;

            var con = factory.CreateConnection();

            var channel = con.CreateModel();

            // Set up exchange
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);

            channel.QueueDeclare(queqeName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            channel.QueueBind(queqeName, exchangeName, routingKey, null);

            channel.BasicGet(queqeName, false);

            // Consuming the message 
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;

                Console.WriteLine("Receiving the messages....");
                Console.WriteLine("");
                Console.WriteLine(Encoding.UTF8.GetString(body));

                Thread.Sleep(3000);

                // manually acknowledge that this consuming was success  
                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queqeName, false, consumer);

            Console.WriteLine("now wating for the messages.........");
        }


        //static void HandleTextMessage(TextMessage textMessage)
        //{
        //    Console.ForegroundColor = ConsoleColor.Red;
        //    Console.WriteLine("Got message: {0}", textMessage.Text);
        //    Console.ResetColor();
        //}
    }
}