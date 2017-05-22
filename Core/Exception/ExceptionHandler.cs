using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StarterPack.Core.Exception
{
    public class ExceptionHandler : ExceptionFilterAttribute
    {

        IHostingEnvironment env;
        public ExceptionHandler(IHostingEnvironment env)
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

                apiError = new ApiError(ex.Message);

                context.HttpContext.Response.StatusCode = ex.StatusCode;
            }
            else if (context.Exception is BusinessException)
            {
                apiError = new ApiError(context.Exception.Message);
                context.HttpContext.Response.StatusCode = 400;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                apiError = new ApiError("messages.notAuthorized");
                context.HttpContext.Response.StatusCode = 403;
            }
            else if (context.Exception is ValidationException)
            {
                Dictionary<String, List<String>> validations = new Dictionary<String, List<String>>();

                foreach (ValidationFailure failure in ((ValidationException)context.Exception).Errors)
                {
                    if (!validations.ContainsKey(failure.PropertyName)) {
                        validations.Add(failure.PropertyName, new List<string>());
                    }

                    validations[failure.PropertyName].Add(failure.ErrorMessage);
                }
                context.HttpContext.Response.StatusCode = 422;
                context.Result = new JsonResult(validations);
            }
            else
            {
                var msg = String.Empty;

                if(this.env.IsDevelopment() || this.env.IsEnvironment("Local"))
                {
                    var error = Regex.Split(context.Exception.GetBaseException().StackTrace.Trim(), @"\s");

                    context.Result = new JsonResult(new {
                        Message = context.Exception.GetBaseException().Message,
                        Source = error[1],
                        Line = error[4],
                        StackTrace = context.Exception.GetBaseException().StackTrace
                    });
                } else {
                    apiError = new ApiError("messages.internalError");
                }

                context.HttpContext.Response.StatusCode = 500;
            }

            if(apiError != null) {
                context.Result = new JsonResult(apiError);
            }

            base.OnException(context);
        }
    }
}
