using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using StarterPack.Core.Helpers;
using StarterPack.Core.Persistence;

namespace StarterPack.Middlewares
{
    public class RequestDbContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestDbContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {            
            Services.DefaultDbContext = (DatabaseContext) context.RequestServices.GetService(typeof(DatabaseContext));
            await this._next(context);
            
            //relase not used threads here    
            int thredId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            if(Services.DbContextByThread.ContainsKey(thredId)) {
                Services.DbContextByThread.Remove(thredId);
            }
        }
    }
}