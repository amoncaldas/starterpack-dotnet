using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StarterPack.Auth;
using StarterPack.Core;
using StarterPack.Models;

namespace StarterPack
{
    public partial class Startup
    {
        public SymmetricSecurityKey signingKey;

        private void ConfigureAuthOptions(IServiceCollection services) {
            //get and config jwt key
            signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Env.Data.GetSection("JWT_KEY").Value));

            //configure jwt token options
            var tokenProviderOptions = new TokenProviderOptions
            {
                Audience = Env.Data.GetSection("APP_URL").Value,
                Issuer = Env.Data.GetSection("APP_NAME").Value,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                Expiration = int.Parse(Env.Data.GetSection("TOKEN_EXPIRATION").Value),
                LeftTimeToRenew = int.Parse(Env.Data.GetSection("TOKEN_LEFT_TIME_TO_REFRESH").Value)
            };

            //check if options is valid
            ThrowIfTokenInvalidOptions(tokenProviderOptions);

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = Env.Data.GetSection("APP_NAME").Value,
                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = Env.Data.GetSection("APP_URL").Value,
                // Validate the token expiry
                ValidateLifetime = true,
                // Set to Zero the difference balance
                ClockSkew = TimeSpan.Zero
            };

            //add options to service injector to use in other places
            services.AddSingleton<TokenProviderOptions>(tokenProviderOptions);
            services.AddSingleton<TokenValidationParameters>(tokenValidationParameters);
        }

        /// <summary>
        /// Check if token options is Valid
        /// </summary>
        /// <param name="options"></param>
        private static void ThrowIfTokenInvalidOptions(TokenProviderOptions options)
        {
            if (string.IsNullOrEmpty(options.Issuer))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Issuer));
            }

            if (string.IsNullOrEmpty(options.Audience))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Audience));
            }

            if (options.Expiration == 0)
            {
                throw new ArgumentException("Deve ser maior que Zero.", nameof(TokenProviderOptions.Expiration));
            }

            if (options.LeftTimeToRenew == 0)
            {
                throw new ArgumentException("Deve ser maior que Zero.", nameof(TokenProviderOptions.LeftTimeToRenew));
            }

            if (options.LeftTimeToRenew > options.Expiration)
            {
                throw new ArgumentException("Deve ser menor que o Expiration.", nameof(TokenProviderOptions.LeftTimeToRenew));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.SigningCredentials));
            }
        }
    }
}
