Light Messaging Core Boilerplate
===

Lightweight core messaging library and boilerplate with _RabbitMQ_ and _MassTransit_

Link: http://www.gokhan-gokalp.com/masstransit-kullanarak-rabbitmq-ile-messaging-altyapisi-olusturma/

Features:
--------
- Provides basic RabbitMQ BUS instance
- Includes basic producer & consumer boilerplate

Manuel Usage:
-----

Firstly you have to add these keys in your configuration file.

```cs
<appSettings>
	<add key="RabbitMQUri" value="rabbitmq://hostAddress/"/>
	<add key="RabbitMQUserName" value="username"/>
	<add key="RabbitMQPassword" value="password"/>
</appSettings>
```

Initializing RabbitMQ BUS instance for _Producer_:

```cs
IBusControl busControl = BusConfigurator.Instance.ConfigureBus();
var sendToUri = new Uri("rabbitmqUri/queueName");

ISendEndpoint bus = busControl.GetSendEndpoint(sendToUri).Result;
```


after RabbitMQ BUS instance initializing then you can use _Send_ method with your queues channel _TCommand_ type.

```cs
bus.Send<TCommand>(new
			{
				SomeProperty = SomeValue
			}
		);
```


_RabbitMQ BUS instance_ using for Consumer:

```cs
static void Main(string[] args)
{
	var bus = BusConfigurator.Instance
		.ConfigureBus((cfg, host) =>
			{
				cfg.ReceiveEndpoint(host, "queueName", e =>
				{
					e.Consumer<TCommandConsumer>();
				});
			});

	bus.Start();

	Console.ReadLine();
}
```


_TCommandConsumer_ could like below:

```cs
public class TCommandConsumer : IConsumer<TCommand>
{
    public async Task Consume(ConsumeContext<TCommand> context)
    {
        var command = context.Message;

		//do something...
        await Console.Out.WriteAsync($"{command.SomeProperty}");
    }
}
```


**PS**: _Publisher_ and _Consumer_ services must be used same _TCommand_ interface. This case important for MassTransit integration.