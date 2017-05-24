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
        /// <summary>
        /// Adiciona ao contexto HTTP o usuário logado
        /// </summary>
        /// <param name="context">Contexto HTTP</param>
        public static void SetCurrentUser(HttpContext context)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //Carrega o usuário com os seus perfis
            context.Items["CurrentUser"] = User.Where(u => u.Id == long.Parse(userId))
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .First();
        }

        /// <summary>
        /// Pega o usuário corrente que foi adicionado ao contexto HTTP
        /// </summary>
        /// <param name="context">Contexto HTTP</param>
        /// <returns>Usuário</returns>
        public static User GetCurrentUser(HttpContext context)
        {
            return (User) context.Items["CurrentUser"];
        }

    }
}
