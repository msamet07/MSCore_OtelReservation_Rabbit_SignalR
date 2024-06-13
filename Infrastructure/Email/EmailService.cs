using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var apiKey = _configuration["Mailjet:ApiKey"];
            var secretKey = _configuration["Mailjet:SecretKey"];
            var requestUri = "https://api.mailjet.com/v3.1/send";

            var email = new
            {
                Messages = new[]
                {
                    new
                    {
                        From = new
                        {
                            Email = "mustafasametdinc07@gmail.com",
                            Name = "Your Name"
                        },
                        To = new[]
                        {
                            new
                            {
                                Email = toEmail,
                                Name = "Recipient Name"
                            }
                        },
                        Subject = subject,
                        TextPart = message,
                        HTMLPart = $"<p>{message}</p>"
                    }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");
            var byteArray = Encoding.ASCII.GetBytes($"{apiKey}:{secretKey}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var response = await _httpClient.PostAsync(requestUri, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error sending email: {response.StatusCode} - {errorMessage}");
            }
        }
    }
}
