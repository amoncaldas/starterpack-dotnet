using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using Humanizer;
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

        public static Dictionary<string, object> GetAttributes() {
            Dictionary<string, object> list = new Dictionary<string, object>();

            Strings.GetSection("attributes").AsEnumerable().ToList().ForEach(x => {
                if(x.Key != "attributes" && x.Value != null) {
                    var fullKey = x.Key.Replace("attributes:", "");
                    var keySplited = fullKey.Split(':');

                    var firstPartKey = keySplited.First().Underscore();

                    if(keySplited.Length == 1) {
                        list[firstPartKey] = x.Value;
                    } else {
                        if (!list.ContainsKey(firstPartKey)) {
                            list[firstPartKey] = new Dictionary<string, object>();
                        }

                        var secondPartKey = keySplited[1].Underscore();

                        //adiciona no model
                        ((Dictionary<string, object>) list[firstPartKey])[secondPartKey] = x.Value;
                    }
                }
            });

            return list;
        }

        public static IConfiguration Strings { get; set; }

    }
}
