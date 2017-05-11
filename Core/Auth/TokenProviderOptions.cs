using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using StarterPack.Models;

namespace StarterPack.Auth
{
    public class TokenProviderOptions
    { 
        /// <summary>
        ///  The Issuer (iss) claim for generated tokens.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// The Audience (aud) claim for the generated tokens.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// The expiration time for the generated tokens.
        /// </summary>
        /// <remarks>The default is 30 minutes.</remarks>
        public int Expiration { get; set; } = 30;

        /// <summary>
        /// The left time for refresh the generated tokens.
        /// </summary>
        /// <remarks>The default is 10 minutes.</remarks>
        public int LeftTimeToRenew { get; set; } = 10;        

        /// <summary>
        /// The signing key to use when generating tokens.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }
    }
}