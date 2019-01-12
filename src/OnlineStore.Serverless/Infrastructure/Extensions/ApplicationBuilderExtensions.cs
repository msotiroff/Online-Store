using Microsoft.AspNetCore.Builder;
using OnlineStore.Serverless.Infrastructure.Middlewares;

namespace OnlineStore.Serverless.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAnonymousBrowser(this IApplicationBuilder app)
        {
            app.UseMiddleware<AnonymousBrowserMiddleware>();
            
            return app;
        }
    }
}
