using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StarterPack.Core.Helpers;
using StarterPack.Core.Persistence;
using StarterPack.Core.Seeders;
using System.Linq;
using StarterPack.Core.Console;

namespace StarterPack.Core.Configure
{
    public class Application
    {
        /// <summary>
        /// Configura os builrders da aplicação responsáveis por prover dados de configuração e recursos de localização
        /// </summary>
        /// <param name="env">String com o tipo de env, como Development, Production, Local, Staging</param>
        /// <param name="contentRootPath">O pasta raiz do conteúdo da aplicação</param>
        public static void ConfigureBuilder(string env, string contentRootPath){
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables();
            IConfigurationRoot Configuration = builder.Build();
            Env.Data = Configuration;

            string culture = "pt_BR";

            var localizationBuilder = new ConfigurationBuilder()
                .SetBasePath(contentRootPath)
                .AddJsonFile($"Resources/Lang/{culture}/messages.json", optional: false)
                .AddJsonFile($"Resources/Lang/{culture}/attributes.json", optional: false);

            Lang.Strings = localizationBuilder.Build();
        }

        /// <summary>
        /// Cria uma colleção de serviços e a definie como singleton no estático Env.Data
        /// </summary>
        /// <param name="env">String com o tipo de env, como Development, Production, Local, Staging</param>
        /// <returns></returns>
        public static ServiceCollection CreateServices(string env){
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(Env.Data);
            return services;
        }

        /// <summary>
        /// Configura o provider da classe <StarterPack.Core.Helpers.Services> a partir de um IServiceCollection passado
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureProvider(IServiceCollection services){
            Services.SetProvider(services.BuildServiceProvider());
            return services;
        }

        /// <summary>
        /// Configura o banco de dados padrão da aplicação
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureDefaultDb(IServiceCollection services){
            services.AddDbContext<Core.Persistence.DefaultDbContext>(
                options => {
                    options.UseNpgsql(Env.Data["DbContextSettings:ConnectionString"]);
                },
                ServiceLifetime.Scoped
            );
        }
    }
}
