using RabbitMQ.Client;
using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace RAbbitMQ.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
          
            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://vxdvxesj:VT6vpW9ieHhgHroI29tDgiM2yqHaXezh@toad.rmq.cloudamqp.com/vxdvxesj");


         
            using (var connnection = factory.CreateConnection())
            {
               

                var channel = connnection.CreateModel();

                //Bir önceki örnekte queue oluşturmuştuk. Bu sefer sadece Exchange oluşturuyorum.
                channel.ExchangeDeclare("logs-fanout",durable:true,type:ExchangeType.Fanout);
              
                foreach (var item in Enumerable.Range(1,125))
                {

                    string message = $"log{item}";

                    var messageBody = Encoding.UTF8.GetBytes(message);

                   
                    channel.BasicPublish("logs-fanout","", null, messageBody);

                    Console.WriteLine($"Mesaj gönderilmiştir : {message}");

                }

                Console.ReadKey();
            }
        }
    }
}
