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
        public static void SetCurrentUser(HttpContext context)   {            
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;


            //get current logged user with roles            
            context.Items["CurrentUser"] = User.Where(u => u.Id == long.Parse(userId)) 
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .First();

            
        }

        public static User GetCurrentUser(HttpContext context) {
            return (User) context.Items["CurrentUser"];
        }
             
    }
}