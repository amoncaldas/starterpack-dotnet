using StarterPack.Models;
using Microsoft.AspNetCore.Mvc;
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
using StarterPack.Core.Controllers;
using StarterPack.Core.Controllers.Attributes;

namespace StarterPack.Controllers
{   
    [Route("api/v1/")]
    [Authorize("authenticatedUser", "check")]
    public class AuthenticationController : BaseController
    {       
        private TokenProviderOptions _tokenProviderOptions;         
        public AuthenticationController(TokenProviderOptions tokenProviderOptions)  {

            _tokenProviderOptions = tokenProviderOptions;
        }
        // POST api/users
        [HttpPost]        
        [Route("authenticate")]    
        public IActionResult login([FromBody]Login login)
        {
            ModelValidator<Login> loginValidator = new ModelValidator<Login>();
            loginValidator.RuleFor(l => l.Email).NotEmpty().EmailAddress();           
            loginValidator.RuleFor(l => l.Password).NotEmpty();

            ValidationResult results = loginValidator.Validate(login);
         
            if(!results.IsValid) {
                throw new Core.Exception.ValidationException(results.Errors);  
            }


            User user = Models.User.BuildQuery(u => u.Email == login.Email).FirstOrDefault();            

            if (user == null || StringHelper.GenerateHash(login.Password + user.Salt) != user.Password)
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
        public IActionResult check() {
            return StatusCode(201);
        }   


        [HttpGet]
        [Route("authenticate/user")]
        public object authenticatedUser()
        {   
            //get current logged user with roles
            User user = CurrentUser();
            
            user.mapToRoles();

            //format render to avoid circular reference
            return new {
                User = user
            };
        }        
    }    
}
