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
            signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("TokenAuthentication:SecretKey").Value));

            //configure jwt token options
            var tokenProviderOptions = new TokenProviderOptions
            {
                Audience = Configuration.GetSection("TokenAuthentication:Audience").Value,
                Issuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                IdentityResolver = GetIdentity
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
                ValidIssuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = Configuration.GetSection("TokenAuthentication:Audience").Value,
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
        /// Check user credentials and return the identity
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private Task<User> GetIdentity(string email, string password)
        {
            User user = User.BuildQuery(u => u.Email == email).FirstOrDefault();            

            if (user != null && StringHelper.GenerateHash(password + user.Salt) == user.Password)
            {
                return Task.FromResult(user);
            }

            // Account doesn't exists or credential invalids
            return Task.FromResult<User>(null);
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

            if (options.Expiration == TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(TokenProviderOptions.Expiration));
            }

            if (options.IdentityResolver == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.IdentityResolver));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.SigningCredentials));
            }
        }              
    }
}