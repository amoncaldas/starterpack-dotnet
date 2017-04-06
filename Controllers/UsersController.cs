using starterpack.Models;
using starterpack.Repository;

namespace starterpack.Controllers
{
    
    public class UsersController : CrudController<User>
    {
        public UsersController(IRepository<User> repository) : base(repository)
        {
            
        }
              
    }
}
