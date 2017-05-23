using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StarterPack.Core.Seeders
{
    public class Seeder
    {
        /// <summary>
        /// Pobvoa os dados de acordo com as regras de uma classe seeder
        /// </summary>
        public static List<string> Populate(List<string> seedersList = null, bool reverseSelection = false){
            return Execute(s => s.InsertData(), seedersList, reverseSelection);
        }

        /// <summary>
        /// Remove os dados de acordo com as regras de uma classe seeder
        /// </summary>
        public static List<string> Reset(List<string> seedersList = null, bool reverseSelection = false){
            return Execute(s => s.EmptyData(), seedersList, reverseSelection);
        }

        public static List<string> Execute(Action<ISeeder> actionMethod, List<string> seedersList, bool reverseSelection = false){

            if(seedersList == null){
                seedersList = new List<string>();
            }

            List<string> seedersExecuted = new List<string>();

            //Recupera a lista de classes que implementam a interface ISeeder
            IEnumerable<Type> types = GetExecutableSeeders();

            if(reverseSelection){
                types = types.Where(t => !seedersList.Contains(t.Name.ToString()));
            }
            else if(seedersList.Count > 0) {
                types = types.Where(t => seedersList.Contains(t.Name.ToString()));
            }

            // Executa a remoção e inserção de dados
            foreach (Type type in types) {
                ISeeder seeder = (ISeeder)Activator.CreateInstance(type);
                actionMethod(seeder);
                seedersExecuted.Add(type.ToString());
            }

            return seedersExecuted;
        }

        /// <summary>
        /// Recupera todas as classes que implementam a interface ISeeder
        /// </summary>
        public static IEnumerable<Type> GetExecutableSeeders(){

            // Recupera o assembly deste pacote
            Assembly assembly = typeof(Seeder).GetTypeInfo().Assembly;

            //Recupera a lista de classes que implementam a interface ISeeder
            IEnumerable<Type> types = assembly.GetTypes().Where(t =>typeof(ISeeder).IsAssignableFrom(t)).Where(t => t.GetTypeInfo().IsClass);

            // Ordena as classes de seeder por nome (a ordem de dependência entre os seeders deve ser resolvida por nome [alfabeticamente])
            types = types.OrderBy(s =>s.GetType().Name);
            return types;
        }
    }
}
