using StarterPack.Models;
using System.Linq;
using StarterPack.Core.Validation;
using FluentValidation;
using StarterPack.Core.Controllers;
using StarterPack.Core.Controllers.Attributes;
using Microsoft.EntityFrameworkCore;

namespace StarterPack.Controllers
{
    [Authorize]
    public class ProjectsController : CrudController<Project>
    {
        protected override void ApplyFilters(ref IQueryable<Project> query) {
            if (HasParameter("name")) {
                query = query.Where(model => model.Name.ToLower().Contains(GetParameter("name").ToLower()));
            }
        }

        protected override void BeforeSearch(ref IQueryable<Project> dataQuery, ref IQueryable<Project> countQuery)
        {
            dataQuery = dataQuery.Include(m => m.Tasks).OrderBy(m => m.Name);
        }

        protected override void SetValidationRules(Project model, ModelValidator<Project> validator) {
            validator.RuleFor(m => m.Name).NotEmpty().Length(10, 100);
            validator.RuleFor(m => m.Cost).NotNull().GreaterThanOrEqualTo(0);
        }
    }
}
