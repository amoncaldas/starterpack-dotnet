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

      public static ServiceCollection ConfigureServices(string env){
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(Env.Data);
            Services.SetProvider(services.BuildServiceProvider());
            return services;
       }



      public static void ConfigureDb(IServiceCollection services){
        services.AddDbContext<Core.Persistence.DefaultDbContext>(
            options => {
                options.UseNpgsql(Env.Data["DbContextSettings:ConnectionString"]);
            },
            ServiceLifetime.Scoped
        );

      }
    }
}
