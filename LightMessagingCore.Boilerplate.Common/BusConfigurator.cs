using System;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace LightMessagingCore.Boilerplate.Common
{
    public class BusConfigurator
    {
        private static readonly Lazy<BusConfigurator> _Instance = new Lazy<BusConfigurator>(() => new BusConfigurator());

        private BusConfigurator()
        {

        }

        public static BusConfigurator Instance => _Instance.Value;

        public IBusControl ConfigureBus(
    Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> registrationAction = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(MqConstants.RabbitMQUri), hst =>
                {
                    hst.Username(MqConstants.RabbitMQUserName);
                    hst.Password(MqConstants.RabbitMQPassword);
                });

                registrationAction?.Invoke(cfg, host);
            });
        }
    }
}