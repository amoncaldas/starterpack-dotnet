using System.Collections.Generic;
using Microsoft.Extensions.CommandLineUtils;
using StarterPack.Core.Seeders;
using System;
using System.Linq;
using System.Diagnostics;
using StarterPack.Core.Console.Runner;
using System.Runtime.InteropServices;
using  System.IO;
using  System.IO.Compression;
using StarterPack.Core.Console.SOCommands;

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
                    IOSCommand osCommand  = OSCommandFactory.Make();

                    if(ValidateAndClean(osCommand)){
                        if(Generate(osCommand)){
                            Write("info","## 6 - Zipando aplicação...");
                            ZipFile.CreateFromDirectory(deployTempFolder, deployFile);

                            Write("info","## 7 - Excluindo pasta temporária deploy...");
                            DeployCommand(osCommand.DeleteFolder(deployTempFolder));

                            Write("success", $"## Pacote {deployFile} criado com sucesso!");

                            //Aqui vai ser implementada a estratégia de envio via ftp ou ssh
                            //DeployCommand(commands.Upload());
                        }
                    }

                    return 0;
                });
            });
        }

        /// <summary>
        /// Executa um comando do deploy. Exibe no console o comando executado
        /// </summary>
        /// <param name="parameters">Uma coleção de strings que deve ter como primeiro item o comando (que está no path ou caminho para executável) e os restante como parâmetros do comando</param>
        /// <returns></returns>
        private static bool DeployCommand(params string[] parameters){
            return DeployCommand(true, parameters);
        }

        /// <summary>
        /// Executa um comando do deploy, podendo especificar se deve ser verbose.
        /// </summary>
        /// <param name="parameters">Uma coleção de strings que deve ter como primeiro item o comando (que está no path ou caminho para executável) e os restante como parâmetros do comando</param>
        /// <returns>Se foi executado com sucesso, retorna true</returns>
        private static bool DeployCommand(bool verbose, params string[] parameters ){
            ExecResult result = null;

            // Exibindo verbose do comando executado
            if(verbose){
                string cmdWithParameters = String.Join(" ", parameters);
                Write("log", $"## Rodando comando [{cmdWithParameters}]");
            }

            result = Command.Exec(parameters[0], String.Join(" ", parameters.Skip(1).ToArray()));

            if(!result.Success && verbose){
                result.Messages.ForEach(msg =>{
                    Write("error",msg);
                });
            }

            return result.Success;
        }

        /// <summary>
        /// Valida o osCommand e limpa a pasta e arquivo de eventual deploy anterior
        /// </summary>
        /// <param name="osCommand">Um objeto do tipo IDeployCommands que resolve os comandos a serem executadosde acordo com o SO</param>
        /// <returns>Se foi executado com sucesso, retorna true</returns>
        private static bool ValidateAndClean(IOSCommand osCommand){
            bool error = false;
            if(osCommand == null){
                Abort(osCommand, "O utilitário de deploy não está disponível para este sistema operacional.");
                error = true;
            }

            Write("info", "## 1 - Excluindo deploy gerado anteriormente (se existir)...");
            if(!error && !DeployCommand(osCommand.DeleteFolder(deployTempFolder))){
                Abort(osCommand, "Não foi possível excluir a pasta deploy. Verifique as permissões da pasta.");
                error = true;
            }

            if(!error && !DeployCommand(osCommand.DeleteFile(deployFile))){
                Abort(osCommand, "Não foi possível excluir um pacote deploy.zip gerado anterioemente. Verifique as permissões do arquivo.");
                error = true;
            }
            return !error;
        }

        /// <summary>
        /// Gera a pasta com estrutura e conteúdo para o deploy da aplicação
        /// </summary>
        /// <param name="osCommand">Um objeto do tipo IDeployCommands que resolve os comandos a serem executadosde acordo com o SO</param>
        /// <returns>Se foi gerado com sucesso, retorna true</returns>
        private static bool Generate(IOSCommand osCommand){
            bool error = false;

            Write("info","## 2 - Compilando a aplicação...");
            if(!DeployCommand("dotnet", $"publish -c=Production --output={deployTempFolder}")) {
                Abort(osCommand, "Não foi possível compilar a aplicação");
                error = true;
            }

            if(!error){
                Write("info","## 3 - Executando Minificação de js e css...");
                if(!DeployCommand(osCommand.Gulp("--gulpfile=public/client/gulpfile.js", "--production"))){
                    Abort(osCommand, $"Não foi possível executar o comando gulp. Verifique se este módulo node está instalado e se está no path.");
                    error = true;
                }
            }

            if(!error){
                Write("info",$"## 4 - Criando pasta {deployTempFolder}/public/client...");
                if(!DeployCommand(osCommand.CreateFolder($"{deployTempFolder}/public/client"))){
                    Abort(osCommand, $"Não foi possível criar a pasta {deployTempFolder}/public/client. Verifique a permissão.");
                    error = true;
                }
            }

            if(!error){
                Write("info","## 5 - Copiando arquivos do client...");
                bool images = DeployCommand(osCommand.Copy("public/client/images/", $"{deployTempFolder}/public/client/"));
                bool app = DeployCommand(osCommand.Copy("public/client/app/", $"{deployTempFolder}/public/client/"));
                bool build = DeployCommand(osCommand.Copy("public/client/build/", $"{deployTempFolder}/public/client/"));
                bool styles = DeployCommand(osCommand.Copy("public/client/styles/", $"{deployTempFolder}/public/client/"));
                if(!images || !app || !build || !styles){
                    Abort(osCommand, $"Não foi possível criar a pasta {deployTempFolder}/public/client. Verifique a permissão.");
                    error = true;
                }
            }
            return !error;
        }

        /// <summary>
        /// Exibe uma mensagem de aborte e exclui os arquivos que tiverem sido gerados até o momento
        /// </summary>
        /// <param name="osCommand">IOsCommand a ser usado para excluir arquivos criados</param>
        /// <param name="msg">mensagem a ser exibida</param>
        private static void Abort(IOSCommand osCommand, string msg){
            msg = "## Erro: " + msg + " Abortado.";
            Write("error", msg);
            DeployCommand(false, osCommand.DeleteFolder(deployTempFolder));
            DeployCommand(false, osCommand.DeleteFile(deployFile));
        }


        /// <summary>
        /// Exibe uma mensagem no console, ajustando a cor de acordo com o tipo
        /// </summary>
        /// <param name="type">tipo da mensagem [error|success|info|log]</param>
        /// <param name="msg">mensagem a ser exibida</param>
        private static void Write(string type, string msg){
            switch (type)
            {
                case "error":
                    System.Console.ForegroundColor = System.ConsoleColor.Red;
                    break;
                case "info":
                    System.Console.ForegroundColor = System.ConsoleColor.Blue;
                    break;
                case "success":
                    System.Console.ForegroundColor = System.ConsoleColor.Green;
                    break;
                case "log":
                    System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
                    break;
                default:System.Console.ForegroundColor = System.ConsoleColor.Blue;
                    break;
            }
            System.Console.WriteLine(msg);
            System.Console.ResetColor();
        }
    }
}
