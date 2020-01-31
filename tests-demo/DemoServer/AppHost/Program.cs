using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Globalization;
using TauCode.Mq;
using TauCode.WebApi.Host.EasyNetQ;
using TauCode.Working;

namespace AppHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Inflector.Inflector.SetDefaultCultureFunc = () => new CultureInfo("en-US");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Verbose()
                .CreateLogger();

            var host = CreateWebHostBuilder(args).Build();

            var publisher = host.Services.GetService<IMessagePublisher>();
            var subscriber = host.Services.GetService<IMessageSubscriber>();

            var scope = ((AutofacServiceProvider)host.Services).LifetimeScope;
            var handlerTypes = scope.GetRegisteredMessageHandlerTypes();

            foreach (var handlerType in handlerTypes)
            {
                subscriber.Subscribe(handlerType);
            }

            publisher.Start();
            subscriber.Start();

            host.Run();

            try
            {
                if (subscriber.State != WorkerState.Disposed)
                {
                    subscriber.Dispose();
                }

                if (publisher.State != WorkerState.Disposed)
                {
                    publisher.Dispose();
                }
            }
            catch
            {
                // silently dismiss
            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
