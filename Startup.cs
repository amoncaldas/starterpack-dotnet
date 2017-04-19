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

using StarterPack.Core;
using FluentValidation;
using StarterPack.Core.Validation;


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

            //Configure Authentication Options
            ConfigureAuthOptions(services);

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();        


            var connectionString = Configuration["DbContextSettings:ConnectionString"];

            services.AddDbContext<Models.DatabaseContext>(
                options => options.UseNpgsql(connectionString));

            ValidatorOptions.ResourceProviderType = typeof(ValidationResourceProvider);
            ValidatorOptions.DisplayNameResolver = ValidationResourceProvider.DisplayNameResolver;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            GetServiceLocator.Instance = app.ApplicationServices;

            //Add support to Static Files.
            app.UseStaticFiles(); 

            //Configure Authentication
            ConfigureAuth(app);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();            

            //Configure CORS
            app.UseCors(builder =>
                builder
                    .WithOrigins("*")
                    .WithMethods("POST", "GET", "OPTIONS", "PUT", "DELETE")
                    .WithHeaders("Origin", "Content-Type", "Accept", "Authorization", "X-Requested-With"));
                            
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
