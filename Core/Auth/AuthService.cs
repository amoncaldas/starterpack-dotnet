using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StarterPack.Models;

namespace StarterPack.Core.Auth
{
    public class AuthService
    {   
        public static void SetCurrentUser(HttpContext context, SecurityToken token)   {            
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //get current logged user with roles            
            context.Items["CurrentUser"] = User.BuildQuery(u => u.Id == long.Parse(userId))
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .First();

            context.Items["Token"] = token;
        }

        public static User GetCurrentUser(HttpContext context) {
            return (User) context.Items["CurrentUser"];
        }

        public static User GetToken(HttpContext context) {
            return (User) context.Items["Token"];
        }        
    }
}