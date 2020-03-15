using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TauCode.Cqrs.Mq;
using TauCode.Mq;
using TauCode.Mq.Abstractions;
using TauCode.Mq.EasyNetQ;

namespace TauCode.WebApi.Host.EasyNetQ
{
    public static class WebApiHostEasyNetQExtensions
    {
        public static IAppStartup AddEasyNetQPublisher(
            this IAppStartup appStartup,
            Type domainEventConverterType,
            string connectionString)
        {
            if (appStartup == null)
            {
                throw new ArgumentNullException(nameof(appStartup));
            }

            if (domainEventConverterType != null && !domainEventConverterType.IsAssignableTo<IDomainEventConverter>())
            {
                throw new ArgumentException(
                    $"'{nameof(domainEventConverterType)}' must be either null or implement interface '{typeof(IDomainEventConverter).FullName}'.",
                    nameof(domainEventConverterType));
            }

            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            var builder = appStartup.GetContainerBuilder();

            builder
                .RegisterType<EasyNetQMessagePublisher>()
                .As<IMessagePublisher>()
                .WithProperties(new List<Parameter>
                {
                    new NamedPropertyParameter(nameof(EasyNetQMessagePublisher.ConnectionString), connectionString),
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

        public static IAppStartup AddEasyNetQSubscriber(
            this IAppStartup appStartup,
            Assembly[] handlersAssemblies,
            Type messageHandlerContextFactoryType,
            string connectionString)
        {
            if (appStartup == null)
            {
                throw new ArgumentNullException(nameof(appStartup));
            }

            if (handlersAssemblies == null)
            {
                throw new ArgumentNullException(nameof(handlersAssemblies));
            }

            if (handlersAssemblies.Any(x => x == null))
            {
                throw new ArgumentException($"'{nameof(handlersAssemblies)}' cannot contain nulls.");
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

            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            var builder = appStartup.GetContainerBuilder();

            builder
                .RegisterAssemblyTypes(handlersAssemblies)
                .Where(x => x.IsClosedTypeOf(typeof(IMessageHandler<>)))
                .AsSelf()
                .InstancePerLifetimeScope();

            builder
                .RegisterType(messageHandlerContextFactoryType)
                .As<IMessageHandlerContextFactory>()
                .SingleInstance();

            builder
                .RegisterType<EasyNetQMessageSubscriber>()
                .As<IMessageSubscriber>()
                .WithProperties(new List<Parameter>
                {
                    new NamedPropertyParameter(nameof(EasyNetQMessageSubscriber.ConnectionString), connectionString),
                })
                .SingleInstance();

            return appStartup;
        }

        //public static IAppStartup AddEasyNetQ(
        //    this IAppStartup appStartup,
        //    Assembly handlersAssembly,
        //    Type messageHandlerContextFactoryType,
        //    Type domainEventConverterType,
        //    string rabbitMQConnectionString)
        //{
        //    if (appStartup == null)
        //    {
        //        throw new ArgumentNullException(nameof(appStartup));
        //    }

        //    if (handlersAssembly == null)
        //    {
        //        throw new ArgumentNullException(nameof(handlersAssembly));
        //    }

        //    if (messageHandlerContextFactoryType == null)
        //    {
        //        throw new ArgumentNullException(nameof(messageHandlerContextFactoryType));
        //    }

        //    if (!messageHandlerContextFactoryType.IsAssignableTo<IMessageHandlerContextFactory>())
        //    {
        //        throw new ArgumentException(
        //            $"'{nameof(messageHandlerContextFactoryType)}' must implement interface '{typeof(IMessageHandlerContextFactory).FullName}'.",
        //            nameof(messageHandlerContextFactoryType));
        //    }

        //    if (domainEventConverterType != null && !domainEventConverterType.IsAssignableTo<IDomainEventConverter>())
        //    {
        //        throw new ArgumentException(
        //            $"'{nameof(domainEventConverterType)}' must be either null or implement interface '{typeof(IDomainEventConverter).FullName}'.",
        //            nameof(domainEventConverterType));
        //    }

        //    if (rabbitMQConnectionString == null)
        //    {
        //        throw new ArgumentNullException(nameof(rabbitMQConnectionString));
        //    }

        //    var builder = appStartup.GetContainerBuilder();

        //    builder
        //        .RegisterAssemblyTypes(handlersAssembly)
        //        .Where(x => x.IsClosedTypeOf(typeof(IMessageHandler<>)))
        //        .AsSelf()
        //        .InstancePerLifetimeScope();

        //    builder
        //        .RegisterType<EasyNetQMessagePublisher>()
        //        .As<IMessagePublisher>()
        //        .WithProperties(new List<Parameter>
        //        {
        //            new NamedPropertyParameter(nameof(EasyNetQMessagePublisher.ConnectionString),
        //                rabbitMQConnectionString),
        //        })
        //        .SingleInstance();

        //    builder
        //        .RegisterType(messageHandlerContextFactoryType)
        //        .As<IMessageHandlerContextFactory>()
        //        .SingleInstance();

        //    builder
        //        .RegisterType<EasyNetQMessageSubscriber>()
        //        .As<IMessageSubscriber>()
        //        .WithProperties(new List<Parameter>
        //        {
        //            new NamedPropertyParameter(nameof(EasyNetQMessageSubscriber.ConnectionString),
        //                rabbitMQConnectionString),
        //        })
        //        .SingleInstance();

        //    if (domainEventConverterType != null)
        //    {
        //        builder
        //            .RegisterType(domainEventConverterType)
        //            .As<IDomainEventConverter>()
        //            .SingleInstance();
        //    }

        //    return appStartup;
        //}

        public static IList<Type> GetRegisteredMessageHandlerTypes(this ILifetimeScope scope)
        {
            var list = new List<Type>();

            foreach (var registration in scope.ComponentRegistry.Registrations)
            {
                var services = registration.Services;
                foreach (var service in services)
                {
                    if (service is TypedService typedService)
                    {
                        var serviceType = typedService.ServiceType;
                        if (serviceType.IsClosedTypeOf(typeof(IMessageHandler<>)))
                        {
                            list.Add(serviceType);
                        }
                    }
                }
            }

            return list;
        }
    }
}
