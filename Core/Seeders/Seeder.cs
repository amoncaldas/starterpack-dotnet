using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StarterPack.Core.Seeders
{
    public class Seeder
    {
        /// <summary>
        /// Recupera todas as classes que implementam a interface ISeeder
        /// </summary>
        public static void Execute(){
            // Recupera o assembly deste pacote
            Assembly assembly = typeof(Seeder).GetTypeInfo().Assembly;

            //Recupera a lista de classes que implementam a interface ISeeder
            IEnumerable<Type> types = assembly.GetTypes().Where(t =>typeof(ISeeder).IsAssignableFrom(t)).Where(t => t.GetTypeInfo().IsClass);

            // Ordena as classes de seeder por nome (a ordem de dependência entre os seeders deve ser resolvida por nome [alfabeticamente])
            types.OrderBy(s =>s.GetType().Name);


            // Executa a remoção e inserção de dados
            foreach (Type type in types) {
                ISeeder seeder = (ISeeder)Activator.CreateInstance(type);
                seeder.EmptyData();
                seeder.InsertData();
            }
        }
    }
}
