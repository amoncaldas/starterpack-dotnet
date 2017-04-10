using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StarterPack.Models;

namespace StarterPack.Controllers
{
    [Route("api/[controller]")]
    public abstract class CrudController<T> : Controller where T : Model<T>
    {
       
        public CrudController()
        {
            
        }

        // GET api/users
        [HttpGet]
        public IEnumerable<T> Get()
        {
            return Model<T>.GetAll();            
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public virtual T Get(long id)
        {
            return Model<T>.Get(id);
        }

        // POST api/users
        [HttpPost]
        public IActionResult Post([FromBody]T model)
        {
            model.Save();
            return StatusCode(201, model);
        } 

        // PUT api/values/5
        [HttpPut("{id}")]
        public T Put(int id, [FromBody]T modelWithAttr)
        {  
            return Model<T>.UpdateAttributes(id, modelWithAttr);            
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Model<T>.Delete(id);           
        }               
    }
}
