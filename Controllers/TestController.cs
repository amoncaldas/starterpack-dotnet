using StarterPack.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;


namespace StarterPack.Controllers
{   
    [Route("api/v1/test/")]
    public class TestController : Controller
    {
        [HttpGet]
        [Route("test")]
        public List<User> test() {
            List<User> users = Model<User>.BuildRawQuery("SELECT * FROM \"Users\"")                
                .Where(u => u.Name.Contains("Teste"))
                .ToList();                

            // IQueryable<User> query = Model<User>.Query()
            //     .Select(t => new User{Id = t.Id, Name = t.Name})
            //     .Where(u => u.Name.Contains("Teste"));          

            return users;

            // return Model<User>.GetAll().ToList();
        }        
    }
}