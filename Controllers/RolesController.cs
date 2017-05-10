using StarterPack.Models;
using System.Linq;
using StarterPack.Core.Validation;
using FluentValidation;
using StarterPack.Core.Controllers;
using StarterPack.Core.Controllers.Attributes;


namespace StarterPack.Controllers
{
    [UnpublishAction("index","update", "destroy")]
  
    public class RolesController : CrudController<Role>
    {
        public RolesController() {
            // Http Request data can be accessed using the folowing code
            // HttpContext.Request;
        } 

        protected override void ApplyFilters(ref IQueryable<Role> query) { 
            if( HttpContext.Request.Query.ContainsKey("slug") ) {
                query = query.Where(role => role.Slug == HttpContext.Request.Query["slug"].First());
            }
            if( HttpContext.Request.Query.ContainsKey("id") ) {
                query = query.Where(role => role.Id == long.Parse(HttpContext.Request.Query["id"].First()));
            }
        }

        protected override void BeforeValidate(Role model, ModelValidator<Role> validator) { 
            model.Slug = model.Title.ToLower();                      
        }

           
        protected override void SetValidationRules(Role model, ModelValidator<Role> validator) {            
            validator.RuleFor(role => role.Title).NotEmpty().EmailAddress();
            validator.RuleFor(user => user.Slug).NotEmpty();
        }       
    }
}
