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
            //pega e configura a chave do jwt
            signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Env.Data.GetSection("JWT_KEY").Value));

            //configura as opções do token
            var tokenProviderOptions = new TokenProviderOptions
            {
                Audience = Env.Data.GetSection("APP_URL").Value,
                Issuer = Env.Data.GetSection("APP_NAME").Value,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                Expiration = int.Parse(Env.Data.GetSection("TOKEN_EXPIRATION").Value),
                LeftTimeToRenew = int.Parse(Env.Data.GetSection("TOKEN_LEFT_TIME_TO_REFRESH").Value)
            };

            ThrowIfTokenInvalidOptions(tokenProviderOptions);

            var tokenValidationParameters = new TokenValidationParameters
            {
                //Valida se a chave está correta
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                //Valida se o emissor está correto
                ValidateIssuer = true,
                ValidIssuer = Env.Data.GetSection("APP_NAME").Value,
                //valida se o dominio está correto
                ValidateAudience = true,
                ValidAudience = Env.Data.GetSection("APP_URL").Value,
                //Valida o tempo de expiração
                ValidateLifetime = true,
                //Define para zero a tolerância para distorção no tempo
                ClockSkew = TimeSpan.Zero
            };

            //adiciona as configurações e opções de validação do token para a injeção de dependência
            services.AddSingleton<TokenProviderOptions>(tokenProviderOptions);
            services.AddSingleton<TokenValidationParameters>(tokenValidationParameters);
        }

        /// <summary>
        /// Verifica se a configuração do token está correta
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
