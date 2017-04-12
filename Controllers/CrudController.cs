using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StarterPack.Models;
using System.Dynamic;
using System.Linq.Expressions;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            var predicate = this.WhereFilter;  
            predicate =  (o => o.Id == model.Id);
            bool trackModels = false;
            BeforeAll(ref trackModels);
            IQueryable<T> query = Model<T>.Query();    
            query.Include(m => m.CreatedAt);      
            BeforeSearch(query, ref trackModels);

            // TODO: put here applyFilters           
            List<T> models = query.ToList();
            AfterSearch(query, models);
            AfterAll();
            return models;            
        }

        public Expression<Func<T, bool>> WhereFilter
        {
            get { return x=>true; }
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
            BeforeSave(model, ref trackModel);
            model.Save();
            AfterSave(model);
            AfterAll();
            return StatusCode(201, model);
        } 

       
        public virtual void update(long id, [FromBody]ExpandoObject attributes)
        {  
            T model = Model<T>.Get(id);
            bool trackModel = false;
            BeforeAll(ref trackModel);
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
         
    }
}
