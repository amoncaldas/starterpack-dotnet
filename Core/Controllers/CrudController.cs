using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using StarterPack.Core.Validation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using StarterPack.Core.Models;
using StarterPack.Core.Extensions;

namespace StarterPack.Core.Controllers
{
    [Route("api/v1/[controller]")]
    public abstract partial class CrudController<T> : BaseController where T : Model<T>
    {
        // GET api/users
        [HttpGet]
        public object Index([FromQuery]T model)
        {       

            bool trackModels = false;
            BeforeAll();
            IQueryable<T> dataQuery = Model<T>.Query();
                    
            ApplyFilters(ref dataQuery);            

            IQueryable<T> countQuery = dataQuery.AsQueryable();

            BeforeSearch(ref dataQuery, ref countQuery, ref trackModels);
            
            if (!trackModels)
                dataQuery = dataQuery.AsNoTracking();

            List<T> models;
            int? count = null;

            if (HasParameter("page") && HasParameter("perPage")) {
                dataQuery = dataQuery.Paginate(GetParameter<int>("page"), GetParameter<int>("perPage"));
                count = countQuery.Count();
            } else {
                if (HasParameter("limit")) {
                    dataQuery = dataQuery.Take(GetParameter<int>("limit"));
                }                 
            }

            models = dataQuery.Fetch();

            AfterSearch(ref dataQuery, models);
            AfterAll();

            if(count == null) {
                return models;
            } else {
                return new {
                    Items = models,
                    Total = count
                };
            }                            
        }       


        // GET api/users/5
        [HttpGet("{id}")]
        public virtual T Get(long id)
        {
            bool trackModel = false;
            BeforeAll();            
            BeforeGet(id, ref trackModel);
            T model = GetSingle(id, trackModel);
            AfterGet(model);           
            AfterAll();
            return model;
        }

        // POST api/users
        [HttpPost]
        public IActionResult Store([FromBody]T model)
        {
            BeforeAll();
            Validate(model);
            BeforeStore(model);
            BeforeSave(model);
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
            T model = GetSingle(id, true);
            model.MergeAttributes(attributes);

            BeforeAll();            
            Validate(model);
            BeforeUpdate(model, attributes); 
            BeforeSave(model);
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
            BeforeAll();
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
        
        protected virtual T GetSingle(long id, bool tracked = true) {
            return Model<T>.Get(id, tracked);
        }                   
         
    }
}
