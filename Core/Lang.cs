using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace StarterPack.Core
{
    public class Lang
    {
        public static string Get(string key, params string[] args) {            
            var value = Strings[key];
            if(value != null) {
                var pattern = @"{(.*?)}";
                var matches = Regex.Matches(value, pattern);
                if(matches.Count > 0 && matches.Count == args.Length) {
                    value = string.Format(value,args);
                }
                return value;                
            }           
            return key;
        }

        public static IConfiguration Strings { get; set; }
        
    }
}