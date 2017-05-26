using System;
using System.Security.Cryptography;
using System.Text;
using Serilog;

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

        public static string GenerateSalt()
        {
            byte[] bytes = new byte[20];
            using(var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        public static string GeneratePassword(int length = 8)
        {
            return Guid.NewGuid().ToString().Substring(0, length);
        }

        public static string GenerateHash(string text)
        {
            // SHA512 is disposable by inheritance.
            using(var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                // Get the hashed string.
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        /// <summary>
		/// Converte o arquivo especificado em uma representação no formato base64
		/// </summary>
		/// <param name="absolutePath">Caminho absoluto do arquivo no FileSystem</param>
		/// <returns>Representação do arquivo em uma string base64</returns>
        public static string FileToBase64String(string absolutePath){
            string imageRepresentation = null;
            try{
                byte[] fileArray = System.IO.File.ReadAllBytes(absolutePath);
                imageRepresentation = Convert.ToBase64String(fileArray).Replace("= ","");
            }
            catch(System.Exception ex){
                Log.Error("It was not possible to convert the file {0} to base64. Error: {1}", absolutePath, ex.Message);
            }
            return imageRepresentation;
        }
    }
}
