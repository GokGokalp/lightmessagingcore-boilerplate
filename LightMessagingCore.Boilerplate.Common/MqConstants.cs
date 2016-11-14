using System.Configuration;

namespace LightMessagingCore.Boilerplate.Common
{
    public class MqConstants
    {
        public static string RabbitMQUri => ConfigurationManager.AppSettings["RabbitMQUri"];
        public static string RabbitMQUserName => ConfigurationManager.AppSettings["RabbitMQUserName"];
        public static string RabbitMQPassword => ConfigurationManager.AppSettings["RabbitMQPassword"];
    }
}