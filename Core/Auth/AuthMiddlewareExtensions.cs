using Microsoft.AspNetCore.Builder;

namespace StarterPack.Auth
{
    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthException(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthExceptionMiddleware>();
        }
    }
}