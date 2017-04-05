using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using starterpack.Models;
using Microsoft.AspNetCore.Mvc;

namespace starterpack.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly DatabaseContext _context;
        public UsersController(DatabaseContext context)
        {
            _context = context;
        }

        // GET api/users
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _context.Users.ToList();
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        // POST api/users
        [HttpPost]
        public IActionResult Post([FromBody]User value)
        {
            _context.Users.Add(value);
            _context.SaveChanges();
            return StatusCode(201, value);
        } 

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]User value)
        {
            var selected = _context.Users.FirstOrDefault(x => x.Id == id);
            if (selected != null)
            {
                _context.Entry(selected).Context.Update(value);
                _context.SaveChanges();
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var selected = _context.Users.FirstOrDefault(x => x.Id == id);
            if (selected != null)
            {
                _context.Users.Remove(selected);
                _context.SaveChanges();
            }
        }               
    }
}
