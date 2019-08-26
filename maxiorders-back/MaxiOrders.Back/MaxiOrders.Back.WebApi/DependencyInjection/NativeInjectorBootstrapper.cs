using AutoMapper;
using MaxiOrders.Back.Common;
using MaxiOrders.Back.Domain.Mapper;
using MaxiOrders.Back.Domain.Services.Companies;
using MaxiOrders.Back.Domain.Services.Master;
using MaxiOrders.Back.Domain.Services.Users;
using MaxiOrders.Back.Domain.UnitOfWork;
using MaxiOrders.Back.Infrastructure.Model.Models;
using MaxiOrders.Back.Infrastructure.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;


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

            // Domain
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IHeadQuarterService, HeadQuarterService>();

            // Infrastructure
            //services.AddScoped<Infrastructure.FrameWork.Loging.ILoggerService, Infrastructure.FrameWork.Loging.LoggerService>();

            // Infra - Data
            services.AddScoped<IDBMaxiOrdersRepositories, DBMaxiOrdersRepositories>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


            //Contexto a db. Adicional se agrga configuración para permitir gurardado de varias peticiones al tiempo. 
            services.AddDbContext<DBMaxiOrdersContext>(options => options.UseSqlServer(configuration.GetConnectionString("MaxiOrdersConnection")));
                //, sqlServerOptionsAction: sqlOptions =>
                //{
                //    sqlOptions.EnableRetryOnFailure(
                //        maxRetryCount: 10,
                //        maxRetryDelay: TimeSpan.FromSeconds(30),
                //        errorNumbersToAdd: null);
                //}
                //), ServiceLifetime.Transient);

            //services.AddTransient<IHttpClientFactoryService, HttpClientFactoryService>();
            serviceProvider = services.BuildServiceProvider();
        }
    }
}
