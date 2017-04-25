using StarterPack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using StarterPack.Auth;
using System.Threading.Tasks;
using StarterPack.Exception;
using StarterPack.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace StarterPack.Controllers
{   
    [Route("api/")]
    [Authorize]
    public class AuthenticationController : Controller
    {       
        private TokenProviderOptions _tokenProviderOptions;
        
        public AuthenticationController(TokenProviderOptions tokenProviderOptions) {
            _tokenProviderOptions = tokenProviderOptions;
        }
        // POST api/users
        [HttpPost]
        [AllowAnonymous]
        [Route("authenticate")]    
        public async Task<object> login([FromBody]Login login)
        {
            User user = await _tokenProviderOptions.IdentityResolver(login.Email, login.Password);

            if (user == null)
            {     
                throw new ApiException("messages.login.invalidCredentials", 401);
            }            

            //Get logged user roles to add in token
            List<string> roles = UserRole.BuildQuery(u => u.UserId == user.Id)
                .Include(ur => ur.Role)
                .AsNoTracking()
                .Select(ur => ur.Role.Slug.ToLower())
                .ToList();

            var token = JwtHelper.Generate(user.Id.Value, roles, _tokenProviderOptions);                

            return StatusCode(201, new { token = token });
        }              

        [HttpGet]
        [Route("authenticate/user")]
        public object authenticatedUser()
        {   
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //get current logged user with roles
            User user = StarterPack.Models.User.BuildQuery(u => u.Id == long.Parse(userId))
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .First();
            
            //format render to avoid circular reference
            return new {
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Roles = user.UserRoles.Select(ur => new {
                    Slug = ur.Role.Slug,
                    Title = ur.Role.Title
                })
            };
        }        
    }    
}
