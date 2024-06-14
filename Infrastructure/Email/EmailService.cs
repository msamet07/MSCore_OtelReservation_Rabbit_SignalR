using System; // Temel .NET türleri ve veri yapılarını kullanmak için gerekli.
using System.Net.Http; // HTTP istek ve yanıtlarıyla çalışmak için gerekli.
using System.Net.Http.Headers; // HTTP başlıklarıyla çalışmak için gerekli.
using System.Text; // Metin kodlamasıyla çalışmak için gerekli.
using System.Threading.Tasks; // Asenkron programlama için gerekli.
using Microsoft.Extensions.Configuration; // Yapılandırma dosyalarını okumak için gerekli.
using Newtonsoft.Json; // JSON serileştirme ve serileştirme işlemleri için gerekli.

namespace Infrastructure.Email // E-posta gönderim işlemleri için alt yapı sınıflarını içeren namespace.
{
    public interface IEmailService // E-posta hizmeti için arayüz.
    {
        Task SendEmailAsync(string toEmail, string subject, string message); // Asenkron e-posta gönderim metodu.
    }

    public class EmailService : IEmailService // IEmailService arayüzünü uygulayan sınıf.
    {
        private readonly IConfiguration _configuration; // Uygulama yapılandırmasını okumak için kullanılan alan.
        private readonly HttpClient _httpClient; // HTTP isteklerini yapmak için kullanılan alan.

        public EmailService(IConfiguration configuration) // Yapılandırma bağımlılığını alır ve HttpClient örneğini oluşturur.
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message) // Asenkron e-posta gönderim metodu.
        {
            var apiKey = _configuration["Mailjet:ApiKey"]; // Mailjet API anahtarı.
            var secretKey = _configuration["Mailjet:SecretKey"]; // Mailjet gizli anahtarı.
            var requestUri = "https://api.mailjet.com/v3.1/send"; // Mailjet API isteği için URI.

            var email = new // Gönderilecek e-posta içeriği.
            {
                Messages = new[]
                {
                    new
                    {
                        From = new
                        {
                            Email = "mustafasametdinc07@gmail.com", // Gönderen e-posta adresi.
                            Name = "Your Name" // Gönderen adı.
                        },
                        To = new[]
                        {
                            new
                            {
                                Email = toEmail, // Alıcı e-posta adresi.
                                Name = "Recipient Name" // Alıcı adı.
                            }
                        },
                        Subject = subject, // E-posta konusu.
                        TextPart = message, // E-posta metin içeriği.
                        HTMLPart = $"<p>{message}</p>" // E-posta HTML içeriği.
                    }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json"); // E-posta içeriğini JSON formatına çevirir.
            var byteArray = Encoding.ASCII.GetBytes($"{apiKey}:{secretKey}"); // API ve gizli anahtarı ASCII formatında kodlar.
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray)); // HTTP isteği için yetkilendirme başlığı ekler.

            var response = await _httpClient.PostAsync(requestUri, content); // E-posta gönderim isteğini yapar.

            if (!response.IsSuccessStatusCode) // Eğer istek başarılı değilse.
            {
                var errorMessage = await response.Content.ReadAsStringAsync(); // Hata mesajını okur.
                throw new HttpRequestException($"Error sending email: {response.StatusCode} - {errorMessage}"); // Hata fırlatır.
            }
        }
    }
}
