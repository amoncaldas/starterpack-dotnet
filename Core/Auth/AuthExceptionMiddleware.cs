using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using System.Linq;
using Newtonsoft.Json;

namespace StarterPack.Auth
{
    public class AuthExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var pipeline = this._next(context);

            if( !context.Response.HasStarted ) { 

                StringValues values;
                
                var hasAuthenticateError = context.Response.Headers.TryGetValue("WWW-Authenticate", out values);
                
                string error = null;

                if( hasAuthenticateError && context.Response.StatusCode == 401 ) {
                    string message = values.First();

                    if(message.Contains("invalid_token")) {
                        error = "token_invalid";
                        
                        if(message.Contains("expired")) { 
                            error = "token_expired";
                        }                         
                    } else if( message.Equals("Bearer") ) {
                        error = "token_not_provided";
                    }
                } else if(context.Response.StatusCode == 403) {
                    error = "messages.notAuthorized";
                }

                if(error != null) {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new 
                    { 
                        error = error
                    }));                
                } else {
                    await pipeline;    
                } 
            } else {
                await pipeline; 
            }           
        }
    }
}