using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using StarterPack.Models;

namespace StarterPack.Controllers
{
    
    public class UsersController : CrudController<User>
    {
        public UsersController() {
            
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public override User Get(long id)
        {
            User user = Models.User.Get(id);
            return user;
        }  
        
        protected override void AfterUpdate(ref User model) {
            dynamic data = new ExpandoObject();
            data.Name = "teste";
            model.UpdateAttributes(data);
        }                  
    }
}
