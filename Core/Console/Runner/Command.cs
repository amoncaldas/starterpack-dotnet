using System.Collections.Generic;
using System.Diagnostics;

namespace StarterPack.Core.Console.Runner
{
    public class Command
    {
        /// <summary>
        /// Executa um comando dotnet via console. O [dotnet] já é implícito, sendo necessário somente informar os comandos filhos de dotnet
        /// </summary>
        /// <param name="command">Comando donet a ser executado, como [publish, build]</param>
        /// <returns></returns>
        public static ExecResult ExecDotNet(string command){
            return Exec(BuildProcess("dotnet", command));
        }

        /// <summary>
        /// Executa um comando via console
        /// </summary>
        /// <param name="proc"></param>
        /// <returns></returns>
        public static ExecResult Exec(string fileToExecute, string arguments){
            return Exec(BuildProcess(fileToExecute, arguments));
        }

        /// <summary>
        /// Executa um processo, que nesse contexto corresponde a um comando
        /// </summary>
        /// <param name="proc"></param>
        /// <returns></returns>
        public static ExecResult Exec(Process proc){
            ExecResult result = new ExecResult(){Success = true};
            try{
                proc.Start();
                while (!proc.StandardOutput.EndOfStream) {
                    string line = proc.StandardOutput.ReadLine();
                    result.Messages.Add(line);
                }
            }
            catch(System.Exception ex){
                result.Success = false;
                result.Messages.Add(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Constrói um processo para ser utilizado na execução. Um processo, nesse contexto, corresponde a um comando
        /// </summary>
        /// <param name="fileToExecute">Caminho do arquivo a ser executado ou nome do comando que esteja no path</param>
        /// <param name="arguments">Argumentos a serem passado para o comando</param>
        /// <returns></returns>
        public static Process BuildProcess(string fileToExecute, string arguments){
             Process proc = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = fileToExecute,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            return proc;
        }
    }
}
