using System;
using LightMessagingCore.Boilerplate.Common;
using System.Configuration;
using MassTransit;

namespace LightMessagingCore.Boilerplate.OrderService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "OrderService";

            var bus = BusConfigurator.Instance
                .ConfigureBus((cfg, host) =>
                {
                    cfg.ReceiveEndpoint(host, ConfigurationManager.AppSettings["OrderQueueName"], e =>
                    {
                        e.Consumer<OrderCommandConsumer>();
                    });
                });

            bus.Start();

            Console.WriteLine("Listening order command..");
            Console.ReadLine();
        }
    }
}