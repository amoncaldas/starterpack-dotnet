using StarterPack.Models;
using System.Linq;
using StarterPack.Core.Validation;
using FluentValidation;
using StarterPack.Core;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD

=======
>>>>>>> c9d0bfe36b6f75c911061579c97d0ffbafd2ed49

namespace StarterPack.Controllers
{    
    public class UsersController : CrudController<User>
    {       

        public UsersController()  {
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
            //mapeia UserRoles para Roles
            models.ForEach(user => {
                user.mapToRoles();
            });
        }

        protected override User GetSingle(long id) {
            return Models.User.BuildQueryById(id)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefault();
        }

        protected override void BeforeStore(User model, ref bool trackModel) {
            //Gera a senha com um hash para evitar que fique igual a senha codificada fique igual para uma mesma senha
            model.Salt = StringHelper.GenerateSalt();
            model.Password = StringHelper.GenerateHash( model.Password + model.Salt );
        }

        //Mapeia Roles para UserRoles
        protected override void BeforeSave(User model, ref bool trackModel) {
            model.mapFromRoles();
        }

        protected override void AfterSave(User model) {
            //Carrega o model com os relacionamentos necessarios para a view
            model = GetSingle(model.Id.Value);
            //mapeia UserRoles para Roles
            model.mapToRoles();
        }           
           
        protected override void SetValidationRules(Model<User> model, ModelValidator<User> validator) {            
            validator.RuleFor(user => user.Email).NotEmpty().EmailAddress();
            validator.RuleFor(user => user.Name).NotEmpty().Length(3,30);  
        }
    }
}
