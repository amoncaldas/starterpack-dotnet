using System.Collections.Generic;
using Microsoft.Extensions.CommandLineUtils;
using StarterPack.Core.Seeders;
using System;
using System.Linq;
using System.Diagnostics;
using StarterPack.Core.Console.Runner;
using StarterPack.Core.Console.Deploy;
using System.Runtime.InteropServices;


// https://www.example-code.com/dotnet-core/ssh_exec.asp
// https://www.nuget.org/packages/SSH.Net.Core/0.9.1-rc

namespace StarterPack.Core.Console
{

    public class DeployListener
    {
        public const string deployFolder = "deploy/";

        /// <summary>
        /// Escuta o comando seed, utilizado para povoar o banco de dados
        /// </summary>
        /// <param name="rootCommand"></param>
        public static void Listen(CommandLineApplication rootCommand, string[] arg){
            rootCommand.Command("deploy", (innerCommand) =>
            {
                innerCommand.HelpOption("--help|-?|-h");

                //Define as descrições do comando
                innerCommand.Description = "Executa o deploy da aplicação, podendo, opcionalmente, enviar o pacote para o FTP configurado em appSettings[env].json";
                innerCommand.FullName = "Executa o deploy da aplicação, podendo, opcionalmente, enviar o pacote para o FTP configurado em appSettings[env].json";
                innerCommand.ExtendedHelpText = "Ex.: dotnet run sp deploy";

                // Cria as três options possíveis: --reset; --only; --exclude
                //var resetOption  = innerCommand.Option("--reset|-r", "Apaga os dados executando o método EmptyData de cada classe Seeder antes de povoar",CommandOptionType.NoValue);
                //var onlyOption  = innerCommand.Option("--only|-o", "Executa somente a lista de Seeders especificadas", CommandOptionType.MultipleValue);
                //var excludeOption  = innerCommand.Option("--exclude|-e", "Executa todas, exceto as Seeders especificadas na lista", CommandOptionType.MultipleValue);

                innerCommand.OnExecute(() =>
                {
                    IDeployCommands commands  = DeployCommandFactory.Make();

                    DeployCommand(commands.Delete(deployFolder, "-Rf"));
                    DeployCommand(commands.Delete("deploy.zip", "-f"));

                    System.Console.ForegroundColor = System.ConsoleColor.Blue;
                    System.Console.WriteLine("## Compilando a aplicação...");
                    System.Console.ResetColor();

                    if( DeployCommand("dotnet", $"publish -c=Production --output={deployFolder}")) {

                        System.Console.ForegroundColor = System.ConsoleColor.Blue;
                        System.Console.WriteLine("## Executando Minificação de js e css...");
                        System.Console.ResetColor();

                        DeployCommand(commands.Gulp("--gulpfile=public/client/gulpfile.js", "--production"));

                        System.Console.ForegroundColor = System.ConsoleColor.Blue;
                        System.Console.WriteLine("## Copiando arquivos do client...");
                        System.Console.ResetColor();

                        DeployCommand(commands.CreateFolder($"{deployFolder}public"));
                        DeployCommand(commands.CreateFolder($"{deployFolder}public/client"));
                        DeployCommand(commands.Copy("public/client/images/", $"{deployFolder}public/client/"));
                        DeployCommand(commands.Copy("public/client/app/", $"{deployFolder}public/client/"));
                        DeployCommand(commands.Copy("public/client/build/", $"{deployFolder}public/client/"));
                        DeployCommand(commands.Copy("public/client/styles/", $"{deployFolder}public/client/"));

                        System.Console.ForegroundColor = System.ConsoleColor.Blue;
                        System.Console.WriteLine("## Zipando aplicação...");
                        System.Console.ResetColor();
                        DeployCommand(commands.Compress("deploy.zip", "deploy/"));

                        System.Console.ForegroundColor = System.ConsoleColor.Green;
                        System.Console.WriteLine("## Pacote Deploy.zip criado com sucesso em!" );
                        System.Console.ResetColor();

                        //Aqui vai ser implementada a estratégia de envio via ftp ou ssh
                        //DeployCommand(commands.Upload());
                    }
                    return 0;
                });
            });
        }

        /// <summary>
        /// Executa um comando do deploy. Se o comando contiver um parâmetro, é executado  <Command.ExecDotNet>, se tiver dois, <Command.Exec>
        /// </summary>
        /// <param name="param1">nome do comando  que vem depois de [dotnet], como [publish, build] ou caminho do arquivo a ser executado</param>
        /// <param name="param2">Parâmetro a ser passado para o arquivo a ser executado</param>
        /// <returns></returns>
        private static bool DeployCommand(params string[] parameters){
            ExecResult result = null;

            // Exibindo verbose do comando executado
            string cmdWithParameters = String.Join(" ", parameters);
            System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
            System.Console.WriteLine($"## Executando {cmdWithParameters}");

            System.Console.ResetColor();

            if(parameters[0] != "dotnet"){
                result = Command.Exec(parameters[0], String.Join(" ", parameters.Skip(1).ToArray()));
            }
            else {
                result = Command.ExecDotNet(String.Join(" ", parameters.Skip(1).ToArray()));
            }
            System.Console.ResetColor();

            if(!result.Success){
                System.Console.ForegroundColor = System.ConsoleColor.Red;
            }
            result.Messages.ForEach(msg =>{
                System.Console.WriteLine(msg);
            });
            return result.Success;
        }
    }
}
