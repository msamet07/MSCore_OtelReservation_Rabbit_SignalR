using Core;
using Core.Entities;
using Core.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Messaging
{
    public interface IMessageService
    {
        void SendMessage(string message);
    }

    public class MessageService : IMessageService
    {
        private readonly ConnectionFactory _factory;
        protected readonly IServiceProvider serviceProvider;

        public MessageService(IConfiguration configuration,IServiceProvider serviceProvider)
        {
            _factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"],
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"]
            };
            this.serviceProvider = serviceProvider;
        }

        public void SendMessage(string message)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: ConstantsProps.QUEUE_NAME, durable: true, exclusive: false, autoDelete: false, arguments: null);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: ConstantsProps.QUEUE_NAME, basicProperties: null, body: body);
            }
        }
       


    }
}

