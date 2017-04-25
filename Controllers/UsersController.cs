using System.Dynamic;
using StarterPack.Models;
using System.Linq;

using StarterPack.Core.Validation;
using FluentValidation;
using StarterPack.Core;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace StarterPack.Controllers
{    
    public class UsersController : CrudController<User>
    {       
        public UsersController(IMapper mapper) : base(mapper) {
            // Http Request data can be accessed using the folowing code
            // HttpContext.Request;
        }     

        protected override void ApplyFilters(ref IQueryable<User> query) {
            query = query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role);

            if( HttpContext.Request.Query["id"].First() != null ) {
                query = query.Where(user => user.Id == long.Parse(HttpContext.Request.Query["id"].First()));
            }
        }

        protected override void AfterSearch(ref IQueryable<User> query, List<User> models) {
            models.ForEach(user => {
                user.mapToRoles();
            });
        }

        protected override void BeforeStore(User model, ref bool trackModel) {
            model.Salt = StringHelper.GenerateSalt();
            model.Password = StringHelper.GenerateHash( model.Password + model.Salt );
        }

        protected override void BeforeSave(User model, ref bool trackModel) {
            model.mapFromRoles();
        }

        protected override void AfterSave(User model) {
            //get current logged user with roles
            List<Role> roles = UserRole.BuildQuery(u => u.UserId == model.Id)
                .AsNoTracking()
                .Select(ur => ur.Role)
                .ToList();

            model.Roles = roles;
        }           
           
        protected override void SetValidationRules(Model<User> model, ModelValidator<User> validator) {            
            validator.RuleFor(user => user.Email).NotEmpty().EmailAddress();
            validator.RuleFor(user => user.Name).NotEmpty().Length(3,30);  
        }

        protected override void BeforeValidate(Model<User> model, ModelValidator<User> validator) {

        }
    }
}
