using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http;
using StarterPack.Core;
using FluentValidation;
using StarterPack.Core.Validation;
using Newtonsoft.Json;
using StarterPack.Core.Renders;
using StarterPack.Core.Exception;
using StarterPack.Core.Helpers;
using StarterPack.Core.Extensions;
using StarterPack.Core.Seeders;
using StarterPack.Core.Configure;

namespace StarterPack
{
    public partial class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Application.ConfigureBuilder(env.EnvironmentName, env.ContentRootPath);
        }
        public IHostingEnvironment env { get; }

        //Use este método para adicionar/configurar serviços ao container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Env.Data);
            Services.SetProvider(services.BuildServiceProvider());
            var builder = services.AddMvc().AddRazorOptions(options => options.ViewLocationExpanders.Add(new ViewLocationExpander()));

            builder.AddMvcOptions(options => {
                //Configura um filtro global para tratar exceptions
                options.Filters.Add(new ExceptionHandler(env));
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

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            //Configura o contexto do banco de dados
            Application.ConfigureDb(services);


            ValidatorOptions.ResourceProviderType = typeof(ValidationResourceProvider);
            ValidatorOptions.DisplayNameResolver = ValidationResourceProvider.DisplayNameResolver;

            services.AddScoped<IViewRenderService, RazorRender>();

            Services.SetProvider(services.BuildServiceProvider());
        }

        // Use este método para configurar o HTTP Request Pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //Configura o arquivo que vai ser chamado por default
            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("client/index.html");

            //Adiciona suporte a arquivos estaticos
            app.UseDefaultFiles(options);
            app.UseStaticFiles();


            loggerFactory.AddConsole(Env.Data.GetSection("Logging"));
            loggerFactory.AddDebug();

            //Configura o CORS
            app.UseCors(builder =>
                builder
                    .WithOrigins("*")
                    .WithMethods("POST", "GET", "OPTIONS", "PUT", "DELETE")
                    .WithHeaders("Origin", "Content-Type", "Accept", "Authorization", "X-Requested-With"));

            //middlaware para db context
            app.UseRequestDbContext();
            app.UseMvc();

            // Uncommenting the line above will enable defining the routes in a central file
            //app.UseMvc(routes => {ApiRoutes.get(routes);});
        }
    }
}
