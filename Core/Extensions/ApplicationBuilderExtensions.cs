using Microsoft.AspNetCore.Builder;
using StarterPack.Auth;

namespace StarterPack.Core.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAuthException(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthExceptionMiddleware>();
        }
    }
}