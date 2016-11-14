using System;
using System.Configuration;
using System.Web.Mvc;
using LightMessagingCore.Boilerplate.Common;
using LightMessagingCore.Boilerplate.OrderUI.Models;
using MassTransit;

namespace LightMessagingCore.Boilerplate.OrderUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly ISendEndpoint _bus;

        public OrderController()
        {
            var busControl = BusConfigurator.Instance.ConfigureBus();
            var sendToUri = new Uri($"{MqConstants.RabbitMQUri}{ConfigurationManager.AppSettings["OrderQueueName"]}");

            _bus = busControl.GetSendEndpoint(sendToUri).Result;
        }

        // GET: Order
        public ActionResult Index(OrderModel orderModel)
        {
            if (orderModel.OrderId > 0)
                CreateOrder(orderModel);

            return View();
        }

        private void CreateOrder(OrderModel orderModel)
        {
            _bus.Send(orderModel).Wait();
        }
    }
}