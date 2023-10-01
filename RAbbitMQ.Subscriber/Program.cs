﻿using Microsoft.AspNet.SignalR.Infrastructure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RAbbitMQ.Subscriber
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
               
                // Publisher tarafında Exchange oluşturdum. Subscriber tarafında tekrar aynı kodu yazmaya gerek yok algılıyor çünki.

                // Buda sabit bir queue oluşturmadık. RAndom bir tane oluşsun istedik. Bu sayede sunbcriber kendi kuyruğunu oluşturucak ama kalıcı olmayacak. İş bitince silinecek. Hava durumu bildirimleri gibi vs durumlarda kullanılması mantıklı
                var randomQueueName = channel.QueueDeclare().QueueName;   
                channel.QueueBind(randomQueueName, "logs-fanout", "", null);           


                channel.BasicQos(0,1,false);

                var consumer = new EventingBasicConsumer(channel);
                           
                channel.BasicConsume(randomQueueName, false,consumer);
                Console.WriteLine("Log alınıyor........");

                consumer.Received += (object sender, BasicDeliverEventArgs e)=> 
                {
                    var message = Encoding.UTF8.GetString(e.Body.ToArray());
                    Console.WriteLine("Gelen Mesaj =======>" + message);

                    channel.BasicAck(e.DeliveryTag, false);
                };

                Console.ReadKey();

            }
            
        }

       
    }
}

