using Microsoft.AspNetCore.Http;
using System.Dynamic;
using StarterPack.Models;
using System.Linq;

namespace StarterPack.Controllers
{
    
    public class UsersController : CrudController<User>
    {

       
        public UsersController()  {
            // Http Request data can be accessed using the folowing code
            // HttpContext.Request;
        }     

        protected override void BeforeGet(long id, ref bool trackModel) {
            long teste = id;
        } 
        
        protected override void AfterUpdate(User model) {
            dynamic data = new ExpandoObject();
            data.Name = "teste";
            model.UpdateAttributes(data);
        } 

        protected override void ApplyFilters(ref IQueryable<User> query) {           
            // query = query.Where(u => u.Id == 1);
            // string sqlafter = query.ToSql();           
        }   

        protected override void BeforeAll(ref bool trackModel) {
            trackModel = true;
        }           

    }
}
