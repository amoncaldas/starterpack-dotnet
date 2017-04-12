using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StarterPack.Models;
using System.Dynamic;
using System.Linq.Expressions;
using System;

namespace StarterPack.Controllers
{
    [Route("api/[controller]")]
    public abstract partial class CrudController<T> : Controller where T : Model<T>
    {  
                
        public CrudController() {
        }

        // GET api/users
        [HttpGet]
        public IEnumerable<T> Index(Expression<Func<T, bool>> predicate)
        {
            bool trackModels = false;
            BeforeAll(ref trackModels); 
            BeforeSearch(ref predicate, ref trackModels);
            IEnumerable<T> models = Model<T>.FindBy(predicate, trackModels);
            AfterSearch(predicate, ref models);
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
            AfterGet(ref model);           
            AfterAll();
            return model;
        }

        // POST api/users
        [HttpPost]
        public IActionResult Store([FromBody]T model)
        {
            bool trackModel = false;
            BeforeAll(ref trackModel);
            BeforeSave(ref model, ref trackModel);
            model.Save();
            AfterSave(ref model);
            AfterAll();
            return StatusCode(201, model);
        } 

       
        public virtual void update(long id, [FromBody]ExpandoObject attributes)
        {  
            T model = Model<T>.Get(id);
            bool trackModel = false;
            BeforeAll(ref trackModel);
            BeforeUpdate(ref model, ref trackModel); 
            model.UpdateAttributes(attributes);
            AfterUpdate(ref model);
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
    }
}
