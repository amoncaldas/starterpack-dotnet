using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StarterPack.Auth;
using StarterPack.Core;
using StarterPack.Models;

namespace StarterPack
{
    public partial class Startup
    {
        public SymmetricSecurityKey signingKey;

        private void ConfigureAuthOptions(IServiceCollection services) {
            signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("TokenAuthentication:SecretKey").Value));

            var tokenProviderOptions = new TokenProviderOptions
            {
                Audience = Configuration.GetSection("TokenAuthentication:Audience").Value,
                Issuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                IdentityResolver = GetIdentity
            };  

            ThrowIfInvalidOptions(tokenProviderOptions);

            services.AddSingleton<TokenProviderOptions>(tokenProviderOptions);           
        }

        private void ConfigureAuth(IApplicationBuilder app)
        {  
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
                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            app.UseExceptionHandler(appBuilder => 
            { 
                appBuilder.Use(async (context, next) => 
                { 
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature; 
        
                    if (error != null && error.Error is SecurityTokenValidationException) 
                    { 
                        context.Response.StatusCode = 401; 
                        context.Response.ContentType = "application/json"; 
        
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new  
                        { 
                            Msg = "token invalid" 
                        })); 
                    } 
                    else if (error != null && error.Error is SecurityTokenExpiredException) 
                    { 
                        context.Response.StatusCode = 401; 
                        context.Response.ContentType = "application/json"; 
        
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new  
                        { 
                            Msg = "token expired" 
                        })); 
                    }                     
                    else if (error != null && error.Error != null) 
                    { 
                        context.Response.StatusCode = 500; 
                        context.Response.ContentType = "application/json"; 
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new 
                        { 
                            Msg = error.Error.Message 
                        })); 
                    } 
                    else await next(); 
                }); 
            }); 

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,                
                TokenValidationParameters = tokenValidationParameters
            });
        }

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

        private static void ThrowIfInvalidOptions(TokenProviderOptions options)
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