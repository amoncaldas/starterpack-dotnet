using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StarterPack.Core.Seeders
{
    public class Seeder
    {
        public static void Execute(){
            // Recupera todas as classes que implementam a interface ISeeder
            IEnumerable<Type> types = typeof(Seeder).GetTypeInfo().Assembly.GetTypes().Where(t =>typeof(ISeeder).IsAssignableFrom(t)).Where(t => t.GetTypeInfo().IsClass);

            // Ordena os seeders por nome
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