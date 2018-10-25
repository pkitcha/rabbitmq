using System;
using EasyNetQ;
using Message;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            #region easynetq
            //using (var bus = RabbitHutch.CreateBus("host=localhost;virtualHost=/;username=guest;password=guest"))
            //{
            //    var input = "";
            //    var Txt = new TextMessage();
            //    Console.WriteLine("Enter a message. 'Quit' to quit.");
            //    while ((input = Console.ReadLine()) != "Quit")
            //    {
            //        Txt.Text = input;
            //        Txt.innerText.Inner1 = "test1";
            //        Txt.innerText.Inner2 = "test2";
            //        Txt.innerText.Inner3 = "test3";


            //        bus.Publish(Txt);
            //    }
            //}
            #endregion

            var userName = "producer";
            var passWord = "producer";
            var hostName = "localhost";
            var exchangeName = "PPI-EXCHANGE";
            var queqeName = "PPI-QUEUE";
            var routingKey = "PPI-ROUTING-KEY";

            var factory = new ConnectionFactory();
            factory.UserName = userName;
            factory.Password = passWord;
            factory.HostName = hostName;

            var con = factory.CreateConnection();
            var channel = con.CreateModel();
            var props = channel.CreateBasicProperties();
            
            props.DeliveryMode = 2;
            
            // Set up exchange
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);

            //Set up Queue and bind the queue with exhange and rounting key
            channel.QueueDeclare(queqeName, durable: true, exclusive: false, autoDelete: false,arguments:null );
            channel.QueueBind(queqeName, exchangeName, routingKey, null);

            //Publish the message
            for (var i = 1; i <= 10; i++)
            {
                var message = new TextMessage("This is testing message!!!!" , i );

                var messageString = JsonConvert.SerializeObject(message);
                var messageByte = Encoding.UTF8.GetBytes(messageString);

                channel.BasicPublish(exchangeName, routingKey, null, messageByte);

                Console.WriteLine(string.Format("publish message to RabbitMQ : {0}", messageString));
            }
            
            channel.Dispose();
            con.Dispose();           

        }
    }
}