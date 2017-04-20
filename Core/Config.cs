using Microsoft.Extensions.Configuration;

namespace StarterPack.Core
{
    public class Config
    {
        public static string Get(string key){
            var value = Data[key];
            if(value != null) {
                return value;
            }
            return key;
        }

        public static IConfiguration Data { get; set; }
    }
}