using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using StarterPack.Exception;
using System;
using StarterPack.Core;
using FluentValidation;
using StarterPack.Core.Validation;

namespace StarterPack
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

            string culture = "pt_BR";
            
            var localizationBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"Resources/Lang/{culture}/messages.json", optional: false)
                .AddJsonFile($"Resources/Lang/{culture}/attributes.json", optional: false);

            Lang.Strings = localizationBuilder.Build();
        }

        public IConfigurationRoot Configuration { get; }

       
        public IHostingEnvironment env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        { 
            services.AddMvc(options => {
                    options.Filters.Add(new ApiExceptionFilter(env));                  
                    
                })               
                .AddJsonOptions(options =>  {                    
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                });                

            //services.AddSingleton<IConfiguration>(sp => { return Configuration; });
            services.AddSingleton<IConfiguration>(localization => { return Lang.Strings; });

            var connectionString = Configuration["DbContextSettings:ConnectionString"];

            services.AddDbContext<Models.DatabaseContext>(
                options => options.UseNpgsql(connectionString)
            ); 

            ValidatorOptions.ResourceProviderType = typeof(ValidationResourceProvider);
            ValidatorOptions.DisplayNameResolver = ValidationResourceProvider.DisplayNameResolver;
            //ValidatorOptions.DisplayNameResolver = ValidationResourceProvider.DisplayPropertyResolver;
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            GetServiceLocator.Instance = app.ApplicationServices;

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseMvc();
            
			
            // Uncommenting the line above will enable defining the routes in a central file
            //app.UseMvc(routes => {ApiRoutes.get(routes);});
        }

        public static class GetServiceLocator
        {
            public static IServiceProvider Instance { get; set; }
        }            
    }
}
