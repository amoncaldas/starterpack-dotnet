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

        // PUT api/values/5
        [HttpPut("{id}")]
        public override void update(int id, [FromBody]ExpandoObject model)
        {  
            BeforeAll();
            // BeforeUpdate(ref model);
            // model.Update();  
            Models.User.UpdateAttributes(id, model);  
            // AfterUpdate(ref model);
            AfterAll();          
        }        

           
              
    }
}
