using MvcUI.Consumer;
using Microsoft.Extensions.Caching.Memory;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcUI
{
    public static class ApplicationBuilderExtentions
    {
        public static RabbitMqMessageConsumer Listener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            
            Listener = app.ApplicationServices.GetService<RabbitMqMessageConsumer>();

            var life = app.ApplicationServices.GetService<Microsoft.Extensions.Hosting.IApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);

            //press Ctrl+C to reproduce if your app runs in Kestrel as a console app
            //life.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            Listener.StartConsuming();
        }
    }
}
