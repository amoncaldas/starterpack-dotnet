using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StarterPack.Core;
using StarterPack.Core.Controllers;

namespace StarterPack.Controllers
{
    [Route("api/v1/support/")]
    public class SupportController : BaseController
    {
        public SupportController() {
            // Http Request data can be accessed using the folowing code
            // HttpContext.Request;
        }

        [HttpGet]
        [Route("langs")]
        public object langs() {
            return new {
                Attributes = Lang.GetAttributes()
            };
        }

        [Route("error")]
        public IActionResult error() {
            throw new Exception("messages.internalError");
        }
    }
}
