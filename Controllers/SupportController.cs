using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace StarterPack.Controllers
{   
    [Route("api/v1/support/")]
    public class SupportController : Controller
    {
        [HttpGet]
        [Route("langs")]
        public List<string> langs() {
            return new List<string>(){};
        }        
    }
}