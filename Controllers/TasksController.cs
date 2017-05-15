using StarterPack.Models;
using System.Linq;
using StarterPack.Core.Validation;
using FluentValidation;
using StarterPack.Core.Controllers;
using StarterPack.Core.Controllers.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Mvc;

namespace StarterPack.Controllers
{
    [Authorize]
    public class TasksController : CrudController<Task>
    {
        protected override void ApplyFilters(ref IQueryable<Task> query) {
            if( HasParameter("description") ) {
                query = query.Where(m => m.Description.ToLower().Contains(GetParameter("description").ToLower()));
            }
            if( HasParameter("id") ) {
                query = query.Where(m => m.Id == GetParameter<long>("id"));
            }
            if( HasParameter("projectId") ) {
                query = query.Where(m => m.Project.Id == GetParameter<long>("projectId"));
            }
            if( HasParameter("priority") ) {
                query = query.Where(m => m.Priority == GetParameter<int>("priority"));
            }
            if( HasParameter("done") ) {
                query = query.Where(m => m.Done == GetParameter<bool>("done"));
            }
            if( HasParameter("dateStart") ) {
                query = query.Where(m => m.ScheduledTo.Value.CompareTo( GetParameter<DateTime>("dateStart")) <= 0);
            }
            if( HasParameter("dateEnd") ) {
                query = query.Where(m => m.ScheduledTo.Value.CompareTo(GetParameter<DateTime>("dateEnd")) >= 0);
            }
        }

        protected override void BeforeSearch(ref IQueryable<Task> dataQuery, ref IQueryable<Task> countQuery)
        {
            dataQuery = dataQuery.OrderBy(m => m.Description);
        }

        protected override void SetValidationRules(Task model, ModelValidator<Task> validator)
        {
            validator.RuleFor(m => m.ProjectId).NotNull();
            validator.RuleFor(m => m.Description).NotEmpty().Length(10, 255);
            validator.RuleFor(m => m.Priority).NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo(10);
            validator.RuleFor(m => m.ScheduledTo).NotNull().GreaterThanOrEqualTo(DateTime.UtcNow);
        }

        [HttpPut, Route("toggleDone")]
        public void ToggleDone([FromBody] Task attributes)
        {
            Task task = Task.Get(attributes.Id.Value);

            task.Done = attributes.Done;

            task.Save();
        }
    }
}
