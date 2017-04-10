using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Starterpack.Exception;
using Starterpack.Models;
using System;

namespace Starterpack
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            this.env = env;
        }

        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services
                .AddMvc(options => {
                        options.Filters.Add(new ApiExceptionFilter(env));
                })
                .AddJsonOptions(options =>  {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                });

            services.AddSingleton<IConfiguration>(sp => { return Configuration; });

            var connectionString = Configuration["DbContextSettings:ConnectionString"];

            services.AddDbContext<Models.DatabaseContext>(
                options => options.UseNpgsql(connectionString)
            );           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            GetMeSomeServiceLocator.Instance = app.ApplicationServices;

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseMvc();
        }

        public static class GetMeSomeServiceLocator
        {
            public static IServiceProvider Instance { get; set; }
        }        
    }
}
