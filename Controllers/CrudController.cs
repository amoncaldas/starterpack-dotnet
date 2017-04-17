using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StarterPack.Models;
using System.Dynamic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StarterPack.Core.Validation;
using StarterPack.Exception;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StarterPack.Controllers
{
    [Route("api/[controller]")]
    public abstract partial class CrudController<T> : Controller where T : Model<T>
    {  
                
        public CrudController() {
        }

        // GET api/users
        [HttpGet]
        public IEnumerable<T> Index([FromQuery]T model)
        {           
            bool trackModels = false;
            BeforeAll(ref trackModels);
            IQueryable<T> query = Model<T>.Query();
           
            BeforeSearch(ref query, ref trackModels);
            ApplyFilters(ref query);            
            
            List<T> models = query.ToList();
            AfterSearch(ref query, models);
            AfterAll();
            return models;            
        }       


        // GET api/users/5
        [HttpGet("{id}")]
        public virtual T Get(long id)
        {
            bool trackModel = false;
            BeforeAll(ref trackModel);            
            BeforeGet(id, ref trackModel);
            T model = Model<T>.Get(id);
            AfterGet(model);           
            AfterAll();
            return model;
        }

        // POST api/users
        [HttpPost]
        public IActionResult Store([FromBody]T model)
        {
            bool trackModel = false;
            BeforeAll(ref trackModel);
            Validate(model);
            BeforeSave(model, ref trackModel);
            model.Save();
            AfterSave(model);
            AfterAll();
            return StatusCode(201, model);
        } 

       // POST api/users
        [HttpPut]
        public virtual void update(long id, [FromBody]ExpandoObject attributes)
        {  
            T model = Model<T>.Get(id);
            bool trackModel = false;
            BeforeAll(ref trackModel);
            Validate(model);
            BeforeUpdate(model, ref trackModel); 
            model.UpdateAttributes(attributes);
            AfterUpdate(model);
            AfterAll();          
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void destroy(long id)
        {
            bool trackModel = false;
            BeforeAll(ref trackModel);
            BeforeDelete(id);
            Model<T>.Delete(id);  
            AfterDelete(id);
            AfterAll();         
        }  

        protected void Validate(T model) {
            BeforeValidate(model);
            ValidationState state = new ValidationState(ModelState);  
            ApiValidationException validationEx = ApiValidationException.Instance;   
            validationEx.ApiErrors.AddRange(state.getErrors());

            List<object> validationRules = getValidationRules(model);
            //run custom validation rules here and add errors to
           
            AfterValidate(model, validationEx); 

            if(state.HasError()) {
                throw validationEx;
            }
        }  
         
    }
}
