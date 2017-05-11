using StarterPack.Models;
using System.Linq;
using StarterPack.Core.Validation;
using FluentValidation;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StarterPack.Mail;
using StarterPack.Core.Controllers;
using StarterPack.Core.Controllers.Attributes;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;

namespace StarterPack.Controllers
{ 
    [Authorize("index:admin", "get:admin", "store:admin", "update:admin", "destroy:admin", "UpdateProfile")]
    public class UsersController : CrudController<User>
    {
        public UsersController() {
            // Http Request data can be accessed using the folowing code
            // HttpContext.Request;
            System.Threading.Thread.CurrentThread.GetHashCode();
        }    

        protected override void ApplyFilters(ref IQueryable<User> query) {
            query = query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role);
 
            if( HttpContext.Request.Query.ContainsKey("id") ) {
                query = query.Where(user => user.Id == long.Parse(HttpContext.Request.Query["id"].First()));
            }           
        }

        protected override void AfterSearch(ref IQueryable<User> query, List<User> models) {
            //mapeia UserRoles para Roles
            models.ForEach(user => {
                user.mapToRoles();
            });
        }
           
        protected override void SetValidationRules(User model, ModelValidator<User> validator) {            
            validator.RuleFor(user => user.Email).NotEmpty().EmailAddress();
            validator.RuleFor(user => user.Name).NotEmpty().Length(3,30);
        }

        protected override User GetSingle(long id, bool tracked = true) {
            return Models.User.WhereId(id, tracked)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefault();
        }

        protected override void BeforeStore(User model) {
            //Gera a senha com um hash para evitar que fique igual a senha codificada fique igual para uma mesma senha
            model.DefinePassword();     
        }

        protected override void AfterStore(User model) {
           new ConfirmRegister(model).Send();          
        } 

        //Mapeia Roles para UserRoles
        protected override void BeforeSave(User model) {
            model.mapFromRoles();
        }

        protected override void AfterSave(User model) {
            //Carrega o model com os relacionamentos necessarios para a view
            model = GetSingle(model.Id.Value);
            //mapeia UserRoles para Roles
            model.mapToRoles();
        }

        [HttpPut]
        [Route("/api/v1/profile")]        
        public virtual IActionResult UpdateProfile([FromBody]User attributes)
        {  
            ModelValidator<User> validator = new ModelValidator<User>();
            validator.RuleFor(o => o.Name).NotEmpty().Length(3,30);
            validator.RuleFor(o => o.Email).NotEmpty().EmailAddress();
            validator.RuleFor(o => o.PasswordConfirmation).Equal(o => o.Password);
            validator.RuleFor(o => o.Password).Length(8,30);

            ValidationResult results = validator.Validate(attributes);
         
            if(!results.IsValid) {
                throw new Core.Exception.ValidationException(results.Errors);  
            }

            User user = CurrentUser();
            user.MergeAttributes(attributes);

            if(!string.IsNullOrEmpty(attributes.Password))
                user.DefinePassword(attributes.Password);

            user.Update();
            user.mapToRoles();

            return StatusCode(201, user);
        }
    }
}
