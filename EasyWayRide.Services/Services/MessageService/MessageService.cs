using RabbitMQ.Client;
using System.Text;

namespace EasyWayRide.Services.Services.MessageService
{
    public class MessageService : IMessageService
    {
        public void PublishMessage(string message, string driverId)
        {
            // Connect to RabbitMQ
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare("driver_exchange", ExchangeType.Topic);

            channel.QueueDeclare(driverId, false, false, false, null);
            channel.QueueBind(driverId, "driver_exchange", "driver.routing.key." + driverId);

            var driverRoutingKey = "driver.routing.key." + driverId;
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("driver_exchange", driverRoutingKey, null, body);

            Console.WriteLine("Response message published successfully.");
        }
    }
}

