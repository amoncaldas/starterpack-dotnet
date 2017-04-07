using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Starterpack.Exception
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
            ApiError apiError = null;

            if (context.Exception is ApiException)
            {
                // handle explicit 'known' API errors
                var ex = context.Exception as ApiException;

                context.Exception = null;
                apiError = new ApiError(ex.Message);

                context.HttpContext.Response.StatusCode = ex.StatusCode;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                apiError = new ApiError("messages.notAuthorized");
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

                apiError = new ApiError(msg);
                context.HttpContext.Response.StatusCode = 500;
            }

            // always return a JSON result
            context.Result = new JsonResult(apiError);

            base.OnException(context);
        }
    }
}