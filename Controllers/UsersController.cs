using Microsoft.AspNetCore.Mvc;
using StarterPack.Models;

namespace StarterPack.Controllers
{
    
    public class UsersController : CrudController<User>
    {
        // GET api/users/5
        [HttpGet("{id}")]
        public override User Get(long id)
        {
            User user = Models.User.Get(id);

            user.name="Thiago1";
            user.Update(user);

            return user;
        }        
              
    }
}
