using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using StarterPack.Core.Controllers;
using StarterPack.Core.Validation;
using StarterPack.Mail;
using StarterPack.Models;

namespace StarterPack.Controllers
{   
    [Route("api/v1/password/")]
    public class PasswordController : BaseController
    {

        public PasswordController(IServiceProvider serviceProvider) : base(serviceProvider) {

        }

        [HttpPost]        
        [Route("email")]
        public object postEmail([FromBody]Login login) {
            ModelValidator<Login> emailValidator = new ModelValidator<Login>();
            emailValidator.RuleFor(l => l.Email).NotEmpty().EmailAddress();
            ValidationResult results = emailValidator.Validate(login);
         
            if(!results.IsValid) {
                throw new ValidationException(results.Errors);  
            }

            User user  = null;
            if(Models.User.BuildQuery(u => u.Email == login.Email).Count() > 0) {
                user = Models.User.BuildQuery(u => u.Email == login.Email).First();
            }            

            if(user != null){
                user.UpdateResetPasswordToken();
                user.Save();
                new RecoveryPassword(user).SendAsync();             
                
                return new {
                    message = "messages.passwordSendingSuccess",                
                };
            } 
            else {
                throw new System.Exception("passwords.resetSendingError");
            }
        } 

        [HttpPost]
        [Route("reset")]       
        public object postReset([FromBody]Login loginReset) {           
            ModelValidator<Login> validator = new ModelValidator<Login>();
            validator.RuleFor(l => l.Email).NotEmpty().EmailAddress();
            validator.RuleFor(l => l.Token).NotEmpty();
            validator.RuleFor(l => l.Password).NotEmpty();
            
            ValidationResult results = validator.Validate(loginReset);
         
            if(!results.IsValid) {
                throw new ValidationException(results.Errors);  
            }

            User user = Models.User.BuildQuery(u => u.Email == loginReset.Email).FirstOrDefault();

            if(user != null){
                user.UpdatePassword(loginReset);  
                user.Save();              
                return new { message = "messages.passwordResetSuccess"};
            } 
            else {
                throw new System.Exception("passwords.resetError");
            }
        }    
    }
}