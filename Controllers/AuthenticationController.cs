using StarterPack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using StarterPack.Auth;
using System.Threading.Tasks;
using StarterPack.Core.Exception;
using StarterPack.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using StarterPack.Core.Validation;
using FluentValidation;
using FluentValidation.Results;

namespace StarterPack.Controllers
{   
    [Route("api/v1/")]
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
            ModelValidator<Login> loginValidator = new ModelValidator<Login>();
            loginValidator.RuleFor(l => l.Email).NotEmpty().EmailAddress();           
            loginValidator.RuleFor(l => l.Password).NotEmpty();

            ValidationResult results = loginValidator.Validate(login);
         
            if(!results.IsValid) {
                throw new Core.Exception.ValidationException(results.Errors);  
            }

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
        [Route("authenticate/check")]
        public void check() {

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
            
            user.mapToRoles();

            //format render to avoid circular reference
            return new {
                User = user
            };
        }        
    }    
}
