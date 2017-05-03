
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using StarterPack.Models;

namespace StarterPack.Core.Auth
{
    public class Auth
    {
        public static User CurrentUser(ClaimsPrincipal userClaims)   {            
            var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier).Value;

            //get current logged user with roles
            return StarterPack.Models.User.BuildQuery(u => u.Id == long.Parse(userId))
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .First();
        }
    }
}