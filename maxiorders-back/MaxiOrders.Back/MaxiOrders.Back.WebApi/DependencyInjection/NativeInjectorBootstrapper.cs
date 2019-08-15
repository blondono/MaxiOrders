using AutoMapper;
using MaxiOrders.Back.Domain.Mapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaxiOrders.Back.WebApi.DependencyInjection
{
    public class NativeInjectorBootStrapper
    {
        /// <summary>
        /// Proveedor de servicios.
        /// </summary>
        public ServiceProvider serviceProvider;

        /// <summary>
        /// Resolver la dependencia de los servicios
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public void RegisterServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddSingleton(configuration);

            //Automapper application
            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            services.AddSingleton(mapper);

            // Add logging
            #region log

            /*

            var columnOptions = new ColumnOptions
            {
                AdditionalDataColumns = new Collection<DataColumn>
               {
                    new DataColumn {DataType =  typeof(int), ColumnName = "Priority"},
                    new DataColumn {DataType = typeof (string), ColumnName = "Title"},
                    new DataColumn {DataType = typeof (string), ColumnName = "MachineName"},
                    new DataColumn {DataType = typeof (string), ColumnName = "AppDomainName"},
                    new DataColumn {DataType = typeof (string), ColumnName = "ProcessID"},
                    new DataColumn {DataType = typeof (string), ColumnName = "ProcessName"},
               }
            };

            columnOptions.Store.Remove(StandardColumn.MessageTemplate);
            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.Message.ColumnName = "Message";
            columnOptions.Level.ColumnName = "Severity";
            columnOptions.TimeStamp.ColumnName = "Timestamp";
            columnOptions.Exception.ColumnName = "FormattedMessage";
            columnOptions.LogEvent.ColumnName = "LogEvent";


            services.AddSingleton<ILogger>(x => new LoggerConfiguration()
             .MinimumLevel.Verbose()
             .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
             .MinimumLevel.Override("System", LogEventLevel.Error)
             .WriteTo.MSSqlServer(configuration["Serilog:ConnectionString"]
             , configuration["Serilog:TableName"]
             , LogEventLevel.Verbose
             , columnOptions: columnOptions
             ).CreateLogger());
        
            */

            #endregion

            // Application
            services.AddScoped<Application.Services.IProcessOrdersApplicationService, Application.Services.ProcessOrdersApplicationService>();
            services.AddScoped<Application.Services.IAuditApplicationService, Application.Services.AuditApplicationService>();
            services.AddScoped<Application.Services.INotificationOrderSavedStatusApplicationService, Application.Services.NotificationOrderSavedStatusApplicationService>();

            //Domain
            services.AddScoped<Domain.Services.IProcessOrdersDomainService, Domain.Services.ProcessOrdersDomainService>();
            services.AddScoped<Domain.Services.IAuditDomainService, Domain.Services.AuditDomainService>();
            services.AddScoped<Domain.Services.INotificationOrderSavedStatusDomainService, Domain.Services.NotificationOrderSavedStatusDomainService>();

            // Infrastructure
            services.AddScoped<Infrastructure.FrameWork.Loging.ILoggerService, Infrastructure.FrameWork.Loging.LoggerService>();
            services.AddScoped<IOrderStatusSqsNotification, OrderStatusSqsNotification>();
            services.AddScoped<ICognitoClient, CognitoClient>();

            // Infra - Data
            services.AddScoped<IDatabaseFactory, DatabaseFactory>();
            services.AddScoped<IDBBackEndCoreRepositories, DBBackEndCoreRepositories>();
            services.AddScoped<IDBCanonicoRepositories, DBCanonicoRepositories>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            //HttpFactoryClient
            //CompleteOrderInformation
            services.AddHttpClient("Pim", client =>
            {
                client.BaseAddress = new Uri(configuration["CompleteOrder:PimEntryPoint"]);
            });
            //InventoryManager
            services.AddHttpClient("InventoryReservation", client =>
            {
                client.BaseAddress = new Uri(configuration["InventoryManager:InventoryReservation:UpdateReservationEntryPoint"]);
            });

            //Contexto a db. Adicional se agrga configuración para permitir gurardado de varias peticiones al tiempo. 
            services.AddDbContext<DBBackendCoreContext>(options => options.UseSqlServer(configuration.GetConnectionString("BackEndCoreConnection")
                , sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }
                ), ServiceLifetime.Transient);

            services.AddTransient<IHttpClientFactoryService, HttpClientFactoryService>();
            serviceProvider = services.BuildServiceProvider();
        }
    }
}
