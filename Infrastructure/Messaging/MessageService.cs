using Microsoft.Extensions.Configuration; // Uygulama yapılandırmasını okumak için gerekli.
using RabbitMQ.Client; // RabbitMQ ile iletişim kurmak için gerekli.
using System.Text; // Metin kodlamasıyla çalışmak için gerekli.

namespace Infrastructure.Messaging // Mesajlaşma işlemleri için alt yapı sınıflarını içeren namespace.
{
    public interface IMessageService // Mesaj hizmeti için arayüz.
    {
        void SendMessage(string message); // Mesaj gönderim metodu.
    }

    public class MessageService : IMessageService // IMessageService arayüzünü uygulayan sınıf.
    {
        private readonly ConnectionFactory _factory; // RabbitMQ bağlantı fabrikasını tutan alan.

        public MessageService(IConfiguration configuration) // Yapılandırma bağımlılığını alır ve ConnectionFactory örneğini oluşturur.
        {
            _factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"], // RabbitMQ sunucusunun adı.
                UserName = configuration["RabbitMQ:UserName"], // RabbitMQ kullanıcı adı.
                Password = configuration["RabbitMQ:Password"] // RabbitMQ parolası.
            };
        }

        public void SendMessage(string message) // Mesaj gönderim metodu.
        {
            using (var connection = _factory.CreateConnection()) // RabbitMQ bağlantısını oluşturur.
            using (var channel = connection.CreateModel()) // Kanal oluşturur.
            {
                channel.QueueDeclare(queue: "reservationQueue", durable: false, exclusive: false, autoDelete: false, arguments: null); // Kuyruk oluşturur veya var olan kuyrukla bağlantı kurar.
                var body = Encoding.UTF8.GetBytes(message); // Mesajı UTF-8 formatında kodlar.

                channel.BasicPublish(exchange: "", routingKey: "reservationQueue", basicProperties: null, body: body); // Mesajı yayınlar.
            }
        }
    }
}
