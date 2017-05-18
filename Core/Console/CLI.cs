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
        /// <summary>
        /// Executa a CLI do StarterPack, permitindo que operações sejam realizadas via linha de comando
        /// </summary>
        /// <param name="args">List ade argumentos passados via command line</param>
        public static void Run(string[] args){

            var app = new CommandLineApplication();
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Application.ConfigureBuilder(env, Directory.GetCurrentDirectory());
            ServiceCollection services =  Application.CreateServices(env);
            Application.ConfigureDefaultDb(services);
            Application.ConfigureProvider(services);

            app.HelpOption("-?|-h|--help");
            ListenForSP(app, args);
            app.Execute(args);
        }

        /// <summary>
        ///Escuta pelo comando raiz do StarterPack CLI, que é o 'sp'
        /// </summary>
        /// <param name="rootCommand"></param>
        public static void ListenForSP(CommandLineApplication rootCommand, string[] args){
            rootCommand.Command("sp", (command) =>
            {
                command.FullName = "CLI do StarterPack. Passe como parâmetro um comando a ser executado";
                command.HelpOption("-?|-h|--help");

                command.OnExecute(() =>
                {
                    command.ShowHelp();
                    return 0;
                });

                SeedListener.Listen(command, args);
            });
        }
    }
}
