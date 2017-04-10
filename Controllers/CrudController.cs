using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Starterpack.Models;

namespace Starterpack.Controllers
{
    [Route("api/[controller]")]
    public abstract class CrudController<T> : Controller where T : BaseModel<T>
    {
        protected readonly BaseModel<T> model;
        public CrudController()
        {
            this.model = BaseModel<T>.createInstance();
        }

        // GET api/users
        [HttpGet]
        public IEnumerable<T> Get()
        {
            return this.model.GetAll();            
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public virtual T Get(long id)
        {
            return BaseModel<T>.Get(id);
        }

        // POST api/users
        [HttpPost]
        public IActionResult Post([FromBody]T value)
        {
            this.model.Insert(value);

            return StatusCode(201, value);
        } 

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]T value)
        {
            var selected = BaseModel<T>.Get(id);            
            if (selected != null)
            {
                this.model.Merge(selected, value);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var selected = BaseModel<T>.Get(id);
            if (selected != null)
            {
                this.model.Delete(selected);
            }
        }               
    }
}
