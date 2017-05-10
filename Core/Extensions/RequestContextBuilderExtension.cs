using Microsoft.AspNetCore.Builder;
using StarterPack.Middlewares;

namespace StarterPack.Core.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseRequestDbContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestDbContextMiddleware>();
        }
    }
}