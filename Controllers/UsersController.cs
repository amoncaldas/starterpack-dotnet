using System.Dynamic;
using StarterPack.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Authorization;

using StarterPack.Core.Validation;
using FluentValidation;
namespace StarterPack.Controllers
{
    [Authorize]
    public class UsersController : CrudController<User>
    {       
        public UsersController()  {
            // Http Request data can be accessed using the folowing code
            // HttpContext.Request;
        }     

        protected override void BeforeGet(long id, ref bool trackModel) {
            long teste = id;
        } 
        
        protected override void AfterUpdate(User model) {
            dynamic data = new ExpandoObject();
            data.Name = "teste";
            model.UpdateAttributes(data);
        } 

        protected override void ApplyFilters(ref IQueryable<User> query) {           
            // query = query.Where(u => u.Id == 1);
            // string sqlafter = query.ToSql();           
        }   

        protected override void BeforeAll(ref bool trackModel) {
            trackModel = true;
        }  
           
        protected override void SetValidationRules(Model<User> model, ModelValidator<User> validator) {
            validator.RuleFor(user => user.Name).NotNull();
            validator.RuleFor(user => user.Password).Length(10,30);            
        }

        protected override void BeforeValidate(Model<User> model, ModelValidator<User> validator) {

        }
    }
}
