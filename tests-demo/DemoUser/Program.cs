using Autofac;
using DemoUser.Cli;
using DemoUser.Cli.Workers;
using DemoUser.Handlers;
using Serilog;
using System;
using TauCode.Cli;
using TauCode.Cli.Exceptions;
using TauCode.Mq;
using TauCode.Mq.Autofac;
using TauCode.Mq.EasyNetQ;

namespace DemoUser
{
    internal class Program
    {
        private class ExitException : Exception
        {

        }

        private IMessagePublisher _publisher;

        private readonly IContainer _container;

        private static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        public Program()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder
                .RegisterType<EasyNetQMessageSubscriber>()
                .As<IMessageSubscriber>()
                .SingleInstance();

            containerBuilder
                .RegisterType<PingHandler>()
                .AsSelf()
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterType<AutofacMessageHandlerContextFactory>()
                .As<IMessageHandlerContextFactory>()
                .SingleInstance();

            containerBuilder
                .RegisterType<EasyNetQMessagePublisher>()
                .As<IMessagePublisher>()
                .SingleInstance();

            containerBuilder
                .RegisterType<SendWorker>()
                .As<ISendWorker>()
                .SingleInstance();

            containerBuilder
                .RegisterType<NameWorker>()
                .As<INameWorker>()
                .SingleInstance();

            containerBuilder
                .RegisterType<AddIn>()
                .AsSelf()
                .SingleInstance();

            containerBuilder
                .RegisterType<Host>()
                .AsSelf()
                .SingleInstance();

            _container = containerBuilder.Build();
        }

        public void Run()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Console()
                .CreateLogger();

            _publisher = _container.Resolve<IMessagePublisher>();
            ((EasyNetQMessagePublisher)_publisher).ConnectionString = "host=localhost";

            var subscriber = _container.Resolve<IMessageSubscriber>();
            ((EasyNetQMessageSubscriber)subscriber).ConnectionString = "host=localhost";

            _publisher.Start();

            var host = _container.Resolve<Host>();
            host.Output = Console.Out;

            host.AddCustomHandler(
                () => throw new ExitException(),
                "exit");

            host.AddCustomHandler(
                Console.Clear,
                "cls");

            while (true)
            {
                try
                {
                    Console.Write(" : ");
                    var line = Console.ReadLine();
                    var command = host.ParseLine(line);
                    host.DispatchCommand(command);
                }
                catch (CliCustomHandlerException)
                {
                    // ignore
                }
                catch (ExitException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            _publisher.Dispose();
        }
    }
}
