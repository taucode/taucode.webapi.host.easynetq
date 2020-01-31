using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using TauCode.Cqrs.Mq;
using TauCode.Mq;
using TauCode.Mq.Abstractions;
using TauCode.Mq.EasyNetQ;

namespace TauCode.WebApi.Host.EasyNetQ
{
    public static class WebApiHostEasyNetQExtensions
    {
        public static IAppStartup AddEasyNetQ(
            this IAppStartup appStartup,
            Assembly handlersAssembly,
            Type messageHandlerContextFactoryType,
            Type domainEventConverterType,
            string rabbitMQConnectionString)
        {
            if (appStartup == null)
            {
                throw new ArgumentNullException(nameof(appStartup));
            }

            if (handlersAssembly == null)
            {
                throw new ArgumentNullException(nameof(handlersAssembly));
            }

            if (messageHandlerContextFactoryType == null)
            {
                throw new ArgumentNullException(nameof(messageHandlerContextFactoryType));
            }

            if (!messageHandlerContextFactoryType.IsAssignableTo<IMessageHandlerContextFactory>())
            {
                throw new ArgumentException(
                    $"'{nameof(messageHandlerContextFactoryType)}' must implement interface '{typeof(IMessageHandlerContextFactory).FullName}'.",
                    nameof(messageHandlerContextFactoryType));
            }

            if (domainEventConverterType != null && !domainEventConverterType.IsAssignableTo<IDomainEventConverter>())
            {
                throw new ArgumentException(
                    $"'{nameof(messageHandlerContextFactoryType)}' must be either null or implement interface '{typeof(IDomainEventConverter).FullName}'.",
                    nameof(domainEventConverterType));
            }

            if (rabbitMQConnectionString == null)
            {
                throw new ArgumentNullException(nameof(rabbitMQConnectionString));
            }

            var builder = appStartup.GetContainerBuilder();

            builder
                .RegisterAssemblyTypes()
                .Where(x => x.IsClosedTypeOf(typeof(IMessageHandler<>)))
                .AsSelf()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<EasyNetQMessagePublisher>()
                .As<IMessagePublisher>()
                .WithProperties(new List<Parameter>
                {
                    new NamedPropertyParameter(nameof(EasyNetQMessagePublisher.ConnectionString),
                        rabbitMQConnectionString),
                })
                .SingleInstance();

            builder
                .RegisterType(messageHandlerContextFactoryType)
                .As<IMessageHandlerContextFactory>()
                .SingleInstance();

            builder
                .RegisterType<EasyNetQMessageSubscriber>()
                .As<IMessageSubscriber>()
                .WithProperties(new List<Parameter>
                {
                    new NamedPropertyParameter(nameof(EasyNetQMessageSubscriber.ConnectionString),
                        rabbitMQConnectionString),
                })
                .SingleInstance();

            if (domainEventConverterType != null)
            {
                builder
                    .RegisterType(domainEventConverterType)
                    .As<IDomainEventConverter>()
                    .SingleInstance();
            }

            return appStartup;
        }
    }
}
