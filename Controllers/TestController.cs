using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarterPack.Core.Controllers;
using System;
using StarterPack.Core.Persistence;
using StarterPack.Core.Helpers;
using System.Threading.Tasks;


namespace StarterPack.Controllers
{   
    [Route("api/v1/test/")]
    public class TestController : BaseController
    {

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


        public TestController() {
            // Http Request data can be accessed using the folowing code
            // HttpContext.Request;
        }      
        

        [HttpGet]
        [Route("test")]
        public string test() {
            string t = Services.DefaultDbContext.GetHashCode().ToString();
            if(HttpContext.Request.Query["id"].First() == "1")
                System.Threading.Thread.Sleep(60*60*5);            
            return Services.DefaultDbContext.GetHashCode().ToString()+"-" + t;
        }
    }
}