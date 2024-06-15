using Core;
using Core.Hubs;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MvcUI.Consumer
{
    public class RabbitMqMessageConsumer:IHostedService
    {
        private readonly IModel model;

        private readonly IServiceProvider serviceProvider;

        public RabbitMqMessageConsumer(IServiceProvider serviceProvider)
        {
            var factory = new ConnectionFactory() { HostName = ConstantsProps.HOST_NAME };
            var connection = factory.CreateConnection();
            model = connection.CreateModel();

            model.QueueDeclare(queue: ConstantsProps.QUEUE_NAME,
                               durable: true, exclusive: false, autoDelete: false, arguments: null);
            this.serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            StartConsuming();
            return Task.CompletedTask;
        }

        public void StartConsuming()
        {
            Console.WriteLine(" [Reservation|MVC] Waiting for messages.");
            var consumer = new EventingBasicConsumer(model);
            object messageResponse = null;

            consumer.Received += (messageModel, ea) =>
            {
                var messageBody = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(messageBody);
                Console.WriteLine($" [Reservation|MVC] Message Received {message}");

                // Get the ChatHub from SignalR (using DI)
                var chatHub = (IHubContext<ReservationHub>)serviceProvider.GetService(typeof(IHubContext<ReservationHub>));

                // Send message to all users in SignalR
                chatHub.Clients.All.SendAsync("reservationCreated", message);
            };
            model.BasicConsume(queue: ConstantsProps.QUEUE_NAME, autoAck: true, consumer: consumer);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
