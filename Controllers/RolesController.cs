using StarterPack.Models;
using System.Linq;
using StarterPack.Core.Validation;
using FluentValidation;
using StarterPack.Core.Controllers;
using StarterPack.Core.Controllers.Attributes;


namespace StarterPack.Controllers
{
    [UnpublishAction("get", "store", "update", "destroy")]
    public class RolesController : CrudController<Role>
    {
        public RolesController() {
            // Http Request data can be accessed using the folowing code
            // HttpContext.Request;
        }

        protected override void ApplyFilters(ref IQueryable<Role> query) {
            if( HasParameter("slug") ) {
                query = query.Where(role => role.Slug == GetParameter("slug"));
            }
            if( HasParameter("id") ) {
                query = query.Where(role => role.Id == GetParameter<long>("id"));
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
