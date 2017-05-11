using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace StarterPack.Core
{
    public class Env
    {
        public static string Config(string key){
            var value = Data[key];
            if(value != null) {
                return value;
            }
            return null;
        }

        public static IConfiguration Data { get; set; }

        public static IHostingEnvironment Host { get; set;}
    }
}