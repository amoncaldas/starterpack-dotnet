﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StarterPack.Exception;

namespace StarterPack.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private String[] values = new string[] { };

        [Route("throw/")]
        public object Throw()
        {
            throw new InvalidOperationException("This is an unhandled exception");
        }

        [Route("throw-api-error/")]
        public object ApiThrow()
        {
            throw new ApiException("api-error-key", 400);
        }        

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return this.values;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return this.values[id];
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
