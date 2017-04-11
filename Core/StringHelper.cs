using System;

namespace StarterPack.Core
{
    public static class StringHelper
    {
        public static string SnakeCaseToTitleCase(this string str)
        {
            var tokens = str.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                tokens[i] = token.Substring(0, 1).ToUpper() + token.Substring(1).ToLower();
            }

            return string.Join("", tokens);
        }
    }
}