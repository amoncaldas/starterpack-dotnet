using System.Collections.Generic;
using Microsoft.Extensions.CommandLineUtils;
using StarterPack.Core.Seeders;
using System;
using System.Linq;
using System.Diagnostics;
using StarterPack.Core.Console.Runner;
using StarterPack.Core.Console.Deploy;
using System.Runtime.InteropServices;
using  System.IO;
using  System.IO.Compression;


// https://www.example-code.com/dotnet-core/ssh_exec.asp
// https://www.nuget.org/packages/SSH.Net.Core/0.9.1-rc

namespace StarterPack.Core.Console
{
    public class DeployListener
    {
        public const string deployTempFolder = "deploy";
        public const string deployFile = "deploy.zip";

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
                
                //var ftpOption  = innerCommand.Option("--send|-s", "Envia o pacote para o ftp";

                innerCommand.OnExecute(() =>
                {
                    System.Console.OutputEncoding = System.Text.Encoding.UTF8;
                    IDeployCommands commands  = DeployCommandFactory.Make();

                    DeployCommand(commands.DeleteFolder(deployTempFolder));                    
                    DeployCommand(commands.DeleteFile(deployFile));                   

                    System.Console.ForegroundColor = System.ConsoleColor.Blue;
                    System.Console.WriteLine("## Compilando a aplicação...");
                    System.Console.ResetColor();

                    if( DeployCommand("dotnet", $"publish -c=Production --output={deployTempFolder}")) {

                        System.Console.ForegroundColor = System.ConsoleColor.Blue;
                        System.Console.WriteLine("## Executando Minificação de js e css...");
                        System.Console.ResetColor();

                        if(DeployCommand(commands.Gulp("--gulpfile=public/client/gulpfile.js", "--production"))){
                            System.Console.ForegroundColor = System.ConsoleColor.Blue;
                            System.Console.WriteLine("## Copiando arquivos do client...");
                            System.Console.ResetColor();
                            
                            Directory.CreateDirectory($"{deployTempFolder}public/client");
                            DeployCommand(commands.Copy("public/client/images/", $"{deployTempFolder}public/client/"));
                            DeployCommand(commands.Copy("public/client/app/", $"{deployTempFolder}public/client/"));
                            DeployCommand(commands.Copy("public/client/build/", $"{deployTempFolder}public/client/"));
                            DeployCommand(commands.Copy("public/client/styles/", $"{deployTempFolder}public/client/"));

                            System.Console.ForegroundColor = System.ConsoleColor.Blue;
                            System.Console.WriteLine("## Zipando aplicação...");
                            System.Console.ResetColor();                        
                            ZipFile.CreateFromDirectory(deployTempFolder, deployFile);                            

                            System.Console.ForegroundColor = System.ConsoleColor.Blue;
                            System.Console.WriteLine("## Excluindo pasta temporária deploy...");
                            System.Console.ResetColor();                           
                            DeployCommand(commands.DeleteFolder(deployTempFolder));

                            System.Console.ForegroundColor = System.ConsoleColor.Green;
                            System.Console.WriteLine($"## Pacote {deployFile} criado com sucesso!" );
                            System.Console.ResetColor();

                            //Aqui vai ser implementada a estratégia de envio via ftp ou ssh
                            //DeployCommand(commands.Upload());
                        }
                        else {
                            System.Console.WriteLine($"## Não foi possível executar o comando gulp. Verifique se este módulo node está instalado e se está no path. Abortado." );
                        }
                        System.Console.ResetColor();

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
            return DeployCommand(true, parameters);
        }

        private static bool DeployCommand(bool verbose, params string[] parameters ){
            ExecResult result = null;

            // Exibindo verbose do comando executado
            if(verbose){
                string cmdWithParameters = String.Join(" ", parameters);
                System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
                System.Console.WriteLine($"## Executando {cmdWithParameters}");
            }
                   
            result = Command.Exec(parameters[0], String.Join(" ", parameters.Skip(1).ToArray()));

            if(!result.Success){
                System.Console.ForegroundColor = System.ConsoleColor.Red;
            }
            if(verbose){
                result.Messages.ForEach(msg =>{
                    System.Console.WriteLine(msg);
                });
            }
            System.Console.ResetColor();
            return result.Success;
        }

    }
}
