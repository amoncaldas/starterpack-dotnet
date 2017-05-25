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
        /// <summary>
        /// Action responsável por retornar a lista de atributos traduzidos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("langs")]
        public object langs() {
            return new {
                Attributes = Lang.GetAttributes()
            };
        }

        /// <summary>
        /// Action utilizada para o redirecionamento de erros fora do pipeline do controller
        /// </summary>
        /// <returns></returns>
        [Route("error")]
        public IActionResult error() {
            throw new Exception("messages.internalError");
        }
    }
}
