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
        protected override void ApplyFilters(ref IQueryable<Role> query) { 
            if( HttpContext.Request.Query.ContainsKey("slug") ) {
                query = query.Where(role => role.Slug == HttpContext.Request.Query["slug"].First());
            }
            if( HttpContext.Request.Query.ContainsKey("id") ) {
                query = query.Where(role => role.Id == long.Parse(HttpContext.Request.Query["id"].First()));
            }
        }

        protected override void BeforeValidate(Model<Role> model, ModelValidator<Role> validator) { 
            ((Role)model).Slug = ((Role)model).Title.ToLower();                      
        }

           
        protected override void SetValidationRules(Model<Role> model, ModelValidator<Role> validator) {            
            validator.RuleFor(role => role.Title).NotEmpty().EmailAddress();
            validator.RuleFor(user => user.Slug).NotEmpty();
        }       
    }
}
