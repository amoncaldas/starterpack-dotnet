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
    }
}
