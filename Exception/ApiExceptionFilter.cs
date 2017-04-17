using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StarterPack.Exception
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {

        IHostingEnvironment env;
        public ApiExceptionFilter(IHostingEnvironment env)
        {
            this.env = env;
        }        

        public override void OnException(ExceptionContext context)
        {
           
            List<ApiError> apiErrors = new List<ApiError>();

            if (context.Exception is ApiException)
            {
                // handle explicit 'known' API errors
                var ex = context.Exception as ApiException;

                context.Exception = null;
                apiErrors.Add(new ApiError(ex.Message));

                context.HttpContext.Response.StatusCode = ex.StatusCode;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                apiErrors.Add(new ApiError("messages.notAuthorized"));
                context.HttpContext.Response.StatusCode = 403;
            }
            else if (context.Exception is ApiValidationException)
            {
                apiErrors = ((ApiValidationException)context.Exception).ApiErrors;
                context.HttpContext.Response.StatusCode = 403;
            }
            else
            {   
                var msg = String.Empty;

                if(this.env.IsDevelopment())
                {
                    msg = context.Exception.GetBaseException().Message;
                } else {
                    msg = "messages.internalError";                      
                }

                apiErrors.Add(new ApiError(msg));
                context.HttpContext.Response.StatusCode = 500;
            }

            // always return a JSON result
            if(apiErrors.Count == 1) {
                context.Result = new JsonResult(apiErrors.First());
            }
            else {
                context.Result = new JsonResult(apiErrors);
            }
           

            base.OnException(context);
        }
    }
}