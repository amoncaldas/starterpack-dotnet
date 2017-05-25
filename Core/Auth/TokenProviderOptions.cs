using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using StarterPack.Models;

namespace StarterPack.Auth
{
    public class TokenProviderOptions
    {
        /// <summary>
        ///  Emissor do token
        /// /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Domain do token
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Tempo de expiração
        /// </summary>
        /// <remarks>O padrão é 30 minutos.</remarks>
        public int Expiration { get; set; } = 30;

        /// <summary>
        /// Definição do tempo restante para renovar um token
        /// </summary>
        /// <remarks>O padrão é 10 minutos</remarks>
        public int LeftTimeToRenew { get; set; } = 10;

        /// <summary>
        /// Chave utilizada para gerar o token
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }
    }
}
