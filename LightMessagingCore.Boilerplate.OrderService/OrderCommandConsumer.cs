using System;
using System.Threading.Tasks;
using LightMessagingCore.Boilerplate.Messaging;
using MassTransit;

namespace LightMessagingCore.Boilerplate.OrderService
{
    public class OrderCommandConsumer : IConsumer<IOrderCommand>
    {
        public async Task Consume(ConsumeContext<IOrderCommand> context)
        {
            var orderCommand = context.Message;

            await Console.Out.WriteAsync($"Order code: {orderCommand.OrderCode} Order id: {orderCommand.OrderId}");

            //do something..
        }
    }
}