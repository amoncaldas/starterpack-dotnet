using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using StarterPack.Auth;

namespace StarterPack.Core
{
    public static class JwtHelper
    {
        public static string Generate(long userId, TokenProviderOptions options, IEnumerable<Claim> additionalClaims = null)  
        {  
            var now = DateTime.UtcNow;

            // Specifically add the jti (nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:
            IEnumerable<Claim> claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),                
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };            

            if(additionalClaims != null)
            {
                claims = claims.Concat(additionalClaims);
            }

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(options.Expiration),
                signingCredentials: options.SigningCredentials);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }                
    }
}