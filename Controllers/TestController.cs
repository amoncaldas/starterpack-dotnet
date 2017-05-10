using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarterPack.Core.Controllers;
using StarterPack.Core.Helpers;

namespace StarterPack.Controllers
{   
    [Route("api/v1/test/")]
    public class TestController : BaseController
    {
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