using StarterPack.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarterPack.Core.Controllers;
using System;
using StarterPack.Core.Models;
using StarterPack.Core.Persistence;
using StarterPack.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace StarterPack.Controllers
{   
    [Route("api/v1/test/")]
    public class TestController : BaseController
    {
        public TestController(IServiceProvider serviceProvider, DatabaseContext context) : base(serviceProvider) {
            // Http Request data can be accessed using the folowing code
            // HttpContext.Request;
        }      

        [HttpGet]
        [Route("testDI")]
        public int testDI() {
            // var serviceScopeFactory = Services.Instance.GetRequiredService<IServiceScopeFactory>();
            // var scope = serviceScopeFactory.CreateScope();

            // var db = scope.ServiceProvider.GetService<DatabaseContext>();
            
            // Console.WriteLine("Numero " + HttpContext.Request.Query["id"].First() + " Hashcode context " + db.GetHashCode());
            Console.WriteLine("Numero " + HttpContext.Request.Query["id"].First() + " Hashcode context" + Services.Resolve<DatabaseContext>().GetHashCode());

            if(HttpContext.Request.Query["id"].First() == "1")
                Task.WaitAll( Task.Delay( 60*60*10 ) );

            if(HttpContext.Request.Query["id"].First() == "2")
                Task.WaitAll( Task.Delay( 60*60*5 ) );

            // db = scope.ServiceProvider.GetService<DatabaseContext>();  

            // Console.WriteLine("Numero " + HttpContext.Request.Query["id"].First() + " Hashcode context " + db.GetHashCode());                
            Console.WriteLine("Numero " + HttpContext.Request.Query["id"].First() + " Hashcode context " + Services.Resolve<DatabaseContext>().GetHashCode());

            return 1;
        }        

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