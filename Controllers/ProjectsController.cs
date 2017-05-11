using StarterPack.Models;
using System.Linq;
using StarterPack.Core.Validation;
using FluentValidation;
using StarterPack.Core.Controllers;
using StarterPack.Core.Controllers.Attributes;


namespace StarterPack.Controllers
{
    public class ProjectsController : CrudController<Project>
    {
        protected override void ApplyFilters(ref IQueryable<Project> query) { 
            if( HttpContext.Request.Query.ContainsKey("name") ) {
                query = query.Where(model => model.Name.Contains(HttpContext.Request.Query["name"].First()));
            }
        }
                   
        protected override void SetValidationRules(Project model, ModelValidator<Project> validator) {            
            validator.RuleFor(m => m.Name).NotEmpty().Length(10, 100);
            validator.RuleFor(m => m.Cost).NotNull().GreaterThanOrEqualTo(0);
        }       
    }
}
