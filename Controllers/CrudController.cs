using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using StarterPack.Models;
using System.Linq.Expressions;


namespace StarterPack.Controllers
{
    [Route("api/[controller]")]
    public abstract partial class CrudController<T> : Controller where T : Model<T>
    {  
        private IHttpContextAccessor _httpContextAccessor;
        public CrudController(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;            
        }

        // GET api/users
        [HttpGet]
        public IEnumerable<T> Index()
        {
            BeforeAll();
            IEnumerable<T> models = Model<T>.GetAll();  
            AfterAll();
            return models;
        }

        [HttpGet]
        public IQueryable<T> Search(Expression<Func<T, bool>> predicate, bool tracked) {       
            BeforeAll(); 
            BeforeSearch(ref predicate);
            IQueryable<T> models = Model<T>.FindBy(predicate, tracked);
            AfterSearch(predicate, ref models);
            AfterAll();
            return models;
        } 

        // GET api/users/5
        [HttpGet("{id}")]
        public virtual T Get(long id)
        {
            BeforeAll();
            BeforeGet(id);
            T model = Model<T>.Get(id);
            AfterGet(ref model);           
            AfterAll();
            return model;
        }

        // POST api/users
        [HttpPost]
        public IActionResult Store([FromBody]T model)
        {
            BeforeAll();
            BeforeSave(ref model);
            model.Save();
            AfterSave(ref model);
            AfterAll();
            return StatusCode(201, model);
        } 

        // PUT api/values/5
        [HttpPut("{id}")]
        public void update(int id, [FromBody]T model)
        {  
            BeforeAll();
            BeforeUpdate(ref model);
            model.Update();  
            AfterUpdate(ref model);
            AfterAll();          
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void destroy(int id)
        {
            BeforeAll();
            BeforeDelete(id);
            Model<T>.Delete(id);  
            AfterDelete(id);
            AfterAll();         
        }               
    }
}
