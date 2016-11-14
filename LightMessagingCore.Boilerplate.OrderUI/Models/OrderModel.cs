using LightMessagingCore.Boilerplate.Messaging;

namespace LightMessagingCore.Boilerplate.OrderUI.Models
{
    public class OrderModel : IOrderCommand
    {
        public string OrderCode { get; set; }

        public int OrderId { get; set; }
    }
}