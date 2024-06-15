Rezervasyon Sistemi Geliştirme

Bu proje, otel rezervasyon sistemi oluşturma, güncelleme, silme ve listeleme gibi işlevleri içermektedir. 
Ayrıca, SignalR kullanarak gerçek zamanlı bildirimler ve RabbitMQ kullanarak mesaj işleme işlevlerini içermektedir. 
Mailjet ile e-posta bildirimleri de entegre edilmiştir.

süreç şu şekilde, rezervasyon oluştuğunda rabbitmq'ye yazılıyor
mvc katmanında rabbitmq mesajını okuyor
signalr üzerinden tüm kullanıcılara bildiriyor

Proje Genel Bakış;

Kullanılan Teknolojiler:
.NET Core
PostgreSQL
RabbitMQ
SignalR
Mailjet
Docker

Proje Kurulumu:

1) Depoyu Klonlayın:
   git clone <repository_url>
   cd <repository_directory>
   
2)Veritabanı Kurulumu:

  PostgreSQL'in kurulu ve çalışır durumda olduğundan emin olun.
  Reservations adında bir veritabanı oluşturun.
  appsettings.json dosyasındaki bağlantı dizgisini güncelleyin:
  
  "ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=Reservations;Username=msdinc;Password=1234"
  }
  
3)RabbitMQ Kurulumu:

  Docker kullanarak RabbitMQ'yu çalıştırın:

  docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management

  appsettings.json dosyasındaki RabbitMQ yapılandırmasını güncelleyin:

  "RabbitMQ": {
  "HostName": "localhost",
  "UserName": "guest",
  "Password": "guest"
  }

4)Mailjet Kurulumu:

  Mailjet hesabınızı oluşturun ve API anahtarlarınızı alın.
  appsettings.json dosyasındaki Mailjet yapılandırmasını güncelleyin:

  "Mailjet": {
  "ApiKey": "YOUR_APİ_KEY",
  "SecretKey": "YOUR_SECRET_KEY"
  }

5)Bağımlılıkları Yükleyin:

  dotnet restore
  
6)Veri Tabanı misyonları:

  dotnet ef migrations add InitialCreate --project Infrastructure
  dotnet ef database update --project Infrastructure

7)Uygulamayı Çalıştırın:

  dotnet run --project WebAPI

