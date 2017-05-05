using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using System;
using Microsoft.AspNetCore.Http;
using StarterPack.Core;
using FluentValidation;
using StarterPack.Core.Validation;
using Newtonsoft.Json;
using StarterPack.Core.Renders;
using StarterPack.Core.Exception;
using StarterPack.Core.Persistence;
using StarterPack.Core.Helpers;

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
            Env.Data = Configuration;  
            Env.Host = env;       

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

        //Use este método para adicionar/configurar serviços ao container
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = services.AddMvc().AddRazorOptions(options => options.ViewLocationExpanders.Add(new ViewLocationExpander())); 
                          
            builder.AddMvcOptions(options => {
                //Configura um filtro global para tratar exceptions
                options.Filters.Add(new ApiExceptionFilter(env));
            });

            builder.AddJsonOptions(options =>  {  
                //Configura a nomenclatura na (de)serialização                  
                options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
                //Ajusta a serialização para evitar que fique em loop
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            //configuração da autenticação
            ConfigureAuthOptions(services);

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();        

            //Configura o contexto do banco de dados
            services.AddDbContext<Core.Persistence.DatabaseContext>(
                options => options.UseNpgsql(Configuration["DbContextSettings:ConnectionString"]));

            ValidatorOptions.ResourceProviderType = typeof(ValidationResourceProvider);
            ValidatorOptions.DisplayNameResolver = ValidationResourceProvider.DisplayNameResolver;

            services.AddScoped<IViewRenderService, RazorRender>();
            
        }

        // Use este método para configurar o HTTP Request Pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Services.Instance = app.ApplicationServices;

            //Configura o arquivo que vai ser chamado por default
            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("client/index.html");

            //Adiciona suporte a arquivos estaticos
            app.UseDefaultFiles(options);
            app.UseStaticFiles(); 

            //Configura a autenticação
            ConfigureAuth(app);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();            

            //Configura o CORS
            app.UseCors(builder =>
                builder
                    .WithOrigins("*")
                    .WithMethods("POST", "GET", "OPTIONS", "PUT", "DELETE")
                    .WithHeaders("Origin", "Content-Type", "Accept", "Authorization", "X-Requested-With"));
                            
            app.UseMvc();
        }         
    }
}
