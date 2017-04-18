using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using StarterPack.Exception;
using System;
using Microsoft.AspNetCore.Http;

namespace StarterPack
{
    public partial class Startup
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
            var builder = services.AddMvc();            

            builder.AddMvcOptions(options => {
                options.Filters.Add(new ApiExceptionFilter(env));
            });

            builder.AddJsonOptions(options =>  {                    
                options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
            });

            ConfigureAuthOptions(services);

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();        

            var connectionString = Configuration["DbContextSettings:ConnectionString"];

            services.AddDbContext<Models.DatabaseContext>(
                options => options.UseNpgsql(connectionString)
            );                       
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            GetServiceLocator.Instance = app.ApplicationServices;

            app.UseStaticFiles(); 

            ConfigureAuth(app);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();            

            app.UseCors(builder =>
                builder
                    .WithOrigins("*")
                    .WithMethods("POST", "GET", "OPTIONS", "PUT", "DELETE")
                    .WithHeaders("Origin", "Content-Type", "Accept", "Authorization", "X-Requested-With"));

            app.UseMvc();
        }

        public static class GetServiceLocator
        {
            public static IServiceProvider Instance { get; set; }
        }        
    }
}
