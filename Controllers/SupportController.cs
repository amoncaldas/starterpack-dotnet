using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StarterPack.Core.Controllers;

namespace StarterPack.Controllers
{   
    [Route("api/v1/support/")]
    public class SupportController : BaseController
    {
        public SupportController(IServiceProvider serviceProvider) : base(serviceProvider) {
            // Http Request data can be accessed using the folowing code
            // HttpContext.Request;
        }         

        [HttpGet]
        [Route("langs")]
        public List<string> langs() {
            return new List<string>(){};
        }        
    }
}