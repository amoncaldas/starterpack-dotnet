using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StarterPack.Models;
using System.Linq;
using StarterPack.Core.Validation;
using FluentValidation.Results;

namespace StarterPack.Core.Controllers
{
    [Route("api/v1/[controller]")]
    public abstract partial class CrudController<T> : Controller where T : Model<T>
    {   
        public CrudController() {
           
        }

        // GET api/users
        [HttpGet]
        public object Index([FromQuery]T model)
        {           
            bool trackModels = false;
            BeforeAll(ref trackModels);
            IQueryable<T> query = Model<T>.Query();
           
            BeforeSearch(ref query, ref trackModels);
            ApplyFilters(ref query);            
            
            List<T> models = query.ToList();
            AfterSearch(ref query, models);
            AfterAll();            
           
            return new {
                Items = models,
                Total = models.Count()
            };            
        }       


        // GET api/users/5
        [HttpGet("{id}")]
        public virtual T Get(long id)
        {
            bool trackModel = false;
            BeforeAll(ref trackModel);            
            BeforeGet(id, ref trackModel);
            T model = GetSingle(id);
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
            BeforeStore(model, ref trackModel);
            BeforeStore(model);
            BeforeSave(model, ref trackModel);
            model.Save();
            AfterStore(model);
            AfterSave(model);
            AfterAll();
            return StatusCode(201, model);
        } 

       // POST api/users
        [HttpPut("{id}")]
        public virtual IActionResult Update(long id, [FromBody]T attributes)
        {  
            T model = GetSingle(id);
            model.MergeAttributes(attributes);
            
            bool trackModel = false;

            BeforeAll(ref trackModel);            
            Validate(model);
            BeforeUpdate(model, attributes, ref trackModel); 
            BeforeSave(model, ref trackModel);
            model.Update();
            AfterUpdate(model);
            AfterSave(model);
            AfterAll();   

            return StatusCode(201, model);       
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Destroy(long id)
        {
            bool trackModel = false;
            BeforeAll(ref trackModel);
            BeforeDelete(id);
            Model<T>.Delete(id);  
            AfterDelete(id);
            AfterAll();         
        }  

        protected void Validate(T model) {           
            ModelValidator<T> modelValidator = new ModelValidator<T>();            
            SetValidationRules(model, modelValidator);
			ValidationResult results = modelValidator.Validate(model);
            BeforeValidate(model, modelValidator);
			Exception.ValidationException validationException = new Exception.ValidationException();           
            validationException.Errors.AddRange(results.Errors);
            bool mustContinue = AfterValidate(model, validationException);

            if(!results.IsValid && mustContinue) {
                throw validationException;
            }
        }  
         
    }
}
