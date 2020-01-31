using Autofac;
using Core;
using DbMigrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using NHibernate.Cfg;
using Persistence;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Reflection;
using TauCode.Cqrs.NHibernate;
using TauCode.Db;
using TauCode.Db.FluentMigrations;
using TauCode.Domain.NHibernate.Types;
using TauCode.Mq.NHibernate;
using TauCode.WebApi.Host;
using TauCode.WebApi.Host.Cqrs;
using TauCode.WebApi.Host.EasyNetQ;
using TauCode.WebApi.Host.NHibernate;

namespace AppHost
{
    public class Startup : AppStartupBase
    {
        protected readonly string SQLiteDbTempFilePath;
        protected readonly string SQLiteConnectionString;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var (item1, item2) = DbUtils.CreateSQLiteConnectionString();
            this.SQLiteDbTempFilePath = item1;
            this.SQLiteConnectionString = item2;

            var migrator = new FluentDbMigrator(
                DbProviderNames.SQLite,
                this.SQLiteConnectionString,
                typeof(M0_Baseline).Assembly);
            migrator.Migrate();
        }

        public IConfiguration Configuration { get; }

        protected override void ConfigureServicesImpl(IServiceCollection services)
        {
            services
                .AddMvc(options =>
                {
                    options.Filters.Add(new ValidationFilterAttribute(this.GetValidatorsAssembly()));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(op =>
                {
                    op.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new Info
                    {
                        Title = "Demo Server RESTful Service",
                        Version = "v1"
                    });
                c.CustomSchemaIds(x => x.FullName);
                c.EnableAnnotations();
            });
        }

        public virtual string GetConnectionString() => this.SQLiteConnectionString;

        private static bool IsRepository(Type type)
        {
            return type.IsClass && type.Name.EndsWith("Repository");
        }

        protected override Assembly GetValidatorsAssembly() => typeof(CoreBeacon).Assembly;

        protected virtual Type GetIdUserType() => typeof(SQLiteIdUserType<>);

        protected virtual Configuration BuildConfiguration()
        {
            var connectionString = this.GetConnectionString();

            var configuration = new Configuration();
            configuration.Properties.Add("connection.connection_string", connectionString);
            configuration.Properties.Add("connection.driver_class", "NHibernate.Driver.SQLite20Driver");
            configuration.Properties.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            configuration.Properties.Add("dialect", "NHibernate.Dialect.SQLiteDialect");

            return configuration;
        }

        protected override void ConfigureContainerBuilder()
        {
            this.AddCqrs(typeof(CoreBeacon).Assembly, typeof(TransactionalCommandHandlerDecorator<>));

            var configuration = this.BuildConfiguration();
            var idUserType = this.GetIdUserType();

            this.AddNHibernate(configuration, typeof(PersistenceBeacon).Assembly, idUserType);

            var containerBuilder = this.GetContainerBuilder();
            containerBuilder.RegisterAssemblyTypes(typeof(PersistenceBeacon).Assembly)
                .Where(IsRepository)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var rabbitMQConnectionString = this.Configuration["ConnectionStrings:RabbitMQConnection"];

            this.AddEasyNetQ(
                typeof(CoreBeacon).Assembly,
                typeof(NHibernateMessageHandlerContextFactory),
                typeof(DomainEventConverter),
                rabbitMQConnectionString);
        }

        public override void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo Server RESTful Service");
            });
            app.UseMvc();
        }
    }
}
