using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;

namespace Infrastructure.Messaging
{
    public interface IMessageService
    {
        void SendMessage(string message);
    }

    public class MessageService : IMessageService
    {
        private readonly ConnectionFactory _factory;

        public MessageService(IConfiguration configuration)
        {
            _factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"],
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"]
            };
        }

        public void SendMessage(string message)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "reservationQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: "reservationQueue", basicProperties: null, body: body);
            }
        }
    }
}

