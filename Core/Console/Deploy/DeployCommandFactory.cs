using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace StarterPack.Core.Console.Deploy
{
    public class DeployCommandFactory
    {
        /// <summary>
        /// Constrói um objeto do tipo IDeployCommand basedo em uma especificada ou a plataforma corrente
        /// </summary>
        /// <param name="platform">Plataforma que deve ser usada como referêcia para construção do IDeployCommand</param>
        /// <returns></returns>
        public static IDeployCommands Make(OSPlatform platform){
            IDeployCommands deployCommandInstance = null;
            // Aqui recuperamos as classes que implmentam a interface IDeployCommand
            Assembly assembly = typeof(DeployCommandFactory).GetTypeInfo().Assembly;
            IEnumerable<Type> types = assembly.GetTypes().Where(t =>typeof(IDeployCommands).IsAssignableFrom(t)).Where(t => t.GetTypeInfo().IsClass);

            // Iteramos sobre elas e verifacamos qual delas tem o OSPlatform desejado
            foreach (Type commandType in types)
            {
               deployCommandInstance = (IDeployCommands)Activator.CreateInstance(commandType);
               if(deployCommandInstance.Platform() == platform){
                   break;
               }
            }

            //Retornamos a instância
            return deployCommandInstance;
        }
        public static IDeployCommands Make(){
            return Make(CurrentOS());
        }

        /// <summary>
        /// Verifica e retorna o OS corrente
        /// </summary>
        /// <returns></returns>
        private static OSPlatform CurrentOS(){
            if(System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux)){
                return OSPlatform.Linux;
            }
            if(System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX)){
                return OSPlatform.Linux;
            }
            return OSPlatform.Windows;
        }
    }
}
