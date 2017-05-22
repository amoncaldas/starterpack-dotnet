using System.Collections.Generic;
using Microsoft.Extensions.CommandLineUtils;
using StarterPack.Core.Seeders;
using System;
using System.Linq;

namespace StarterPack.Core.Console
{
    public class SeedListener
    {
        /// <summary>
        /// Escuta o comando seed, utilizado para povoar o banco de dados
        /// </summary>
        /// <param name="rootCommand"></param>
        public static void Listen(CommandLineApplication rootCommand, string[] arg){
            rootCommand.Command("seed", (innerCommand) =>
            {
                innerCommand.HelpOption("--help|-?|-h");

                //Define as descrições do comando
                innerCommand.Description = "Povoa a base de dados com base nas classes que implementam a interface ISeeder";
                innerCommand.FullName = "Povoa a base de dados com base nas classes que implementam a interface ISeeder";
                innerCommand.ExtendedHelpText = "Ex.: dotnet run sp seed --only=UserSeeder";

                // Cria as três options possíveis: --reset; --only; --exclude
                var resetOption  = innerCommand.Option("--reset|-r", "Apaga os dados executando o método EmptyData de cada classe Seeder antes de povoar",CommandOptionType.NoValue);
                var onlyOption  = innerCommand.Option("--only|-o", "Executa somente a lista de Seeders especificadas", CommandOptionType.MultipleValue);
                var excludeOption  = innerCommand.Option("--exclude|-e", "Executa todas, exceto as Seeders especificadas na lista", CommandOptionType.MultipleValue);

                innerCommand.OnExecute(() =>
                {
                    // Lista de seeders a serem incluída/excluída
                    List<string> seedersList = null;

                    // Por padrão a lista definida é inclusiva
                    bool reverseSelection = false;

                    // Trata o caso da opção de --only, recuperando a lista de Seeders que devem ser executadas
                    if(onlyOption.HasValue()){
                        // Atualmente não é suportado usar em conjunto as opções --only e --exclude em conjunto
                        // Se esse for o caso, notifica o usuário e interrompe a execução
                        if(excludeOption.HasValue()){
                            System.Console.ForegroundColor = System.ConsoleColor.Red;
                            System.Console.WriteLine("## Não é possível usar as opções [--only|-o] e [--exclude|-e] ao mesmo tempo.");
                            System.Console.ResetColor();
                            return 1;
                        }
                        else {
                            seedersList = ParseSeedersListFromOption(onlyOption);
                        }
                    }

                    // Trata o caso da opção de --exclude, recuperando a lista de Seeders que NÃO devem ser executadas
                    if(excludeOption.HasValue()){
                        seedersList = ParseSeedersListFromOption(excludeOption);
                        reverseSelection = true;
                        System.Console.ForegroundColor = System.ConsoleColor.Blue;
                        System.Console.WriteLine("## Executando todas as seeders, exceto: " + String.Join(",", seedersList));
                        System.Console.ResetColor();
                    }

                    // Verifica se deve ser feito a exclusão dos dados antes do seed [--reset]
                    if(resetOption.HasValue()){
                        // Caso sim, executa o reset e imprime a lista de seeders que foram resetadas
                        List<string> seedersReseted = Seeder.Reset(seedersList, reverseSelection);
                        System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
                        seedersReseted.ForEach(s => {
                            System.Console.WriteLine($"{s} reseted");
                        });
                        System.Console.ResetColor();
                    }

                    // Executa o povoamento e imprimir a lista de seeders que foram executadas
                    List<string> seedersPopulated = Seeder.Populate(seedersList, reverseSelection);
                    System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
                    seedersPopulated.ForEach(s => {
                        System.Console.WriteLine($"{s} populated");
                    });
                    System.Console.ResetColor();

                    //Imprimi o relatório de seeders executadas
                    if(seedersPopulated.Count > 0){
                        System.Console.ForegroundColor = System.ConsoleColor.Green;
                        System.Console.WriteLine($"## [{seedersPopulated.Count}] Seeder(s) executada(s) com sucesso");
                    }
                    else {
                        System.Console.ForegroundColor = System.ConsoleColor.Blue;
                        System.Console.WriteLine("## Nenhuma Seeder foi executada");
                    }
                    System.Console.ResetColor();

                    return 0;
                });
            });
        }

        /// <summary>
        /// Extraii a lista de seeders (inclusiva uo exclusiva) de um comando
        /// </summary>
        /// <param name="option">CommandOption</param>
        /// <returns></returns>
        public static List<string> ParseSeedersListFromOption(CommandOption option){

            List<string> values = new List<string>();
            option.Values.ForEach(v =>{
                if(v.Contains(",")){
                   values.AddRange(v.Split(',').ToList());
                }
                else {
                   values.Add(v);
                }
            });
            return values;
        }
    }
}
