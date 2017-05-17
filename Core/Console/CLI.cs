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
using StarterPack.Core.Configure;

namespace StarterPack.Core.Console
{
    public class CLI
    {
        public static void Run(string[] args){

            var app = new CommandLineApplication();
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Application.ConfigureBuilder(env, Directory.GetCurrentDirectory());
            ServiceCollection services =  Application.ConfigureServices(env);
            Application.ConfigureDb(services);
            Services.SetProvider(services.BuildServiceProvider());

            app.HelpOption("-?|-h|--help");
            ListenForSP(app);
            app.Execute(args);
        }

         public static void ListenForSP(CommandLineApplication rootCommand){
            rootCommand.Command("sp", (command) =>
            {
                command.FullName = "CLI do StarterPack. Passe como parâmetro um comando a ser executado";
                command.HelpOption("-?|-h|--help");

                command.OnExecute(() =>
                {
                    command.ShowHelp();
                    return 0;
                });
                ListenForSeed(command);
            });
        }

        public static void ListenForSeed(CommandLineApplication rootCommand){
            rootCommand.Command("seed", (innerCommand) =>
            {
                innerCommand.HelpOption("-?|-h|--help");
                innerCommand.Description = "Povoa a base de dados com base nas classes que implementam a interface ISeeder";
                innerCommand.FullName = "Povoa a base de dados com base nas classes que implementam a interface ISeeder";
                innerCommand.ExtendedHelpText = "Ex.: dotnet run sp seed UserSeed";
                CommandArgument locationArgument = innerCommand.Argument("[location]", "Where the ninja should hide.");
                innerCommand.OnExecute(() =>
                {
                    Seeder.Execute();
                    System.Console.WriteLine("Seeds executada com sucesso");
                    return 0;
                });
            });
        }
    }
}
