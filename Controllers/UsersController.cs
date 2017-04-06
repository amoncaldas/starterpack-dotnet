using Starterpack.Models;
using Starterpack.Repository;

namespace Starterpack.Controllers
{
    
    public class UsersController : CrudController<User>
    {
        public UsersController(IRepository<User> repository) : base(repository)
        {
            
        }
              
    }
}
