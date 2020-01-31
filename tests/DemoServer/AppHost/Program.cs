using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using TauCode.Mq;

namespace AppHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Inflector.Inflector.SetDefaultCultureFunc = () => new CultureInfo("en-US");
            //CreateWebHostBuilder(args).Build().Run();

            var host = CreateWebHostBuilder(args).Build();

            var publisher = host.Services.GetService<IMessagePublisher>();
            var subscriber = host.Services.GetService<IMessageSubscriber>();

            publisher.Start();
            subscriber.Start();

            host.Run();

            subscriber.Dispose();
            publisher.Dispose();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
