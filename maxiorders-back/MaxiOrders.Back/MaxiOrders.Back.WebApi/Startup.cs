using AutoMapper;
using MaxiOrders.Back.Common;
using MaxiOrders.Back.Domain.Services.Users;
using MaxiOrders.Back.Domain.UnitOfWork;
using MaxiOrders.Back.Infrastructure.Repository.Repositories;
using MaxiOrders.Back.WebApi.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Text;

namespace MaxiOrders.Back.WebApi
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        /// <summary>
        /// Ambiente.
        /// /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        /// Configuración de servicios.
        /// </summary>
        /// <param name="services">LIstado IServiceCollection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("AppSettings:SecretKey"));

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


            //Domain
            services.AddScoped<IUserService, UserService>();

            // Infrastructure
            //services.AddScoped<Infrastructure.FrameWork.Loging.ILoggerService, Infrastructure.FrameWork.Loging.LoggerService>();

            // Infra - Data
            services.AddScoped<IDBMaxiOrdersRepositories, DBMaxiOrdersRepositories>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Formatting = Formatting.Indented;
            });

            services.AddAutoMapper(typeof(Startup));
            RegisterServices(services);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder.</param>
        /// <param name="env">IHostingEnvironment.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseAuthentication();
            app.UseMvc();
            app.UseStaticFiles();
        }

        /// <summary>
        /// Registra los servicios.
        /// </summary>
        /// <param name="services">IServiceCollection.</param>
        private void RegisterServices(IServiceCollection services)
        {
            new NativeInjectorBootStrapper().RegisterServices(services, Configuration);
        }
    }
}
