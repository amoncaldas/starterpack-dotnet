using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using StarterPack.Core.Helpers;
using StarterPack.Core.Persistence;

namespace StarterPack.Middlewares
{
    /// <summary>
    /// Middleware responsável por setar o DBContext por thread antes da execução da requisição. E remover depois da execução.
    /// /// </summary>
    public class RequestDbContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestDbContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Services.SetCurrentThreadDbContext((DefaultDbContext)context.RequestServices.GetService(typeof(DefaultDbContext)));

            // run the request
            await this._next(context);

            //remove the context for this request when it finishes
            Services.RemoveCurrentThreadDbContext();
        }
    }
}
