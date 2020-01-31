using Autofac;
using DemoUser.Cli;
using Serilog;
using System;
using TauCode.Cli;
using TauCode.Cli.Exceptions;
using TauCode.Mq;
using TauCode.Mq.EasyNetQ;

namespace DemoUser
{
    internal class Program
    {
        private class ExitException : Exception
        {

        }

        private string _name;
        private IMessagePublisher _publisher;
        private IMessageSubscriber _subscriber;

        private readonly IContainer _container;

        private static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        public Program()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<EasyNetQMessageSubscriber>().As<IMessageSubscriber>().SingleInstance();
            containerBuilder.RegisterType<EasyNetQMessagePublisher>().As<IMessagePublisher>().SingleInstance();

            _container = containerBuilder.Build();

        }

        public void Run()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Console()
                .CreateLogger();

            while (true)
            {
                try
                {
                    Console.Write("Name: ");
                    _name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(_name))
                    {
                        throw new Exception("Bad name.");
                    }

                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            Console.WriteLine($"Node started with name '{_name}'.");

            _publisher = _container.Resolve<IMessagePublisher>();
            ((EasyNetQMessagePublisher)_publisher).ConnectionString = "host=localhost";

            _subscriber = _container.Resolve<IMessageSubscriber>();
            _subscriber.Name = _name;
            ((EasyNetQMessageSubscriber)_subscriber).ConnectionString = "host=localhost";


            _publisher.Start();
            _subscriber.Start();

            var host = new Host
            {
                Output = Console.Out,
                UserName = _name,
                Publisher = _publisher,
            };

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

            _subscriber.Dispose();
            _publisher.Dispose();
        }
    }
}
