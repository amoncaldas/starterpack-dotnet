using System.Collections.Generic;
using System.Linq;
using Starterpack.Models;
using Microsoft.AspNetCore.Mvc;
using Starterpack.Repository;

namespace Starterpack.Controllers
{
    [Route("api/[controller]")]
    public abstract class CrudController<Model> : Controller where Model : BaseModel
    {
        private readonly IRepository<Model> repository;
        public CrudController(IRepository<Model> repository)
        {
            this.repository = repository;
        }

        // GET api/users
        [HttpGet]
        public IEnumerable<Model> Get()
        {
            return this.repository.GetAll();
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public Model Get(long id)
        {
            return this.repository.Get(id);
        }

        // POST api/users
        [HttpPost]
        public IActionResult Post([FromBody]Model value)
        {
            this.repository.Insert(value);

            return StatusCode(201, value);
        } 

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Model value)
        {
            var selected = this.repository.Get(id);
            if (selected != null)
            {
                this.repository.Merge(selected, value);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var selected = this.repository.Get(id);
            if (selected != null)
            {
                this.repository.Delete(selected);
            }
        }               
    }
}
