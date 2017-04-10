using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarterPack.Models;

namespace StarterPack.Controllers
{
    
    public class UsersController : CrudController<User>
    {
        public UsersController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) {
            
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public override User Get(long id)
        {
            User user = Models.User.Get(id);
            return user;
        }  

           
              
    }
}
