using Microsoft.AspNetCore.Http;

namespace OnlineStore.Serverless.Infrastructure.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetBrowserId(this HttpContext context)
        {
            return context.Request.Cookies[WebConstants.AnonymounsBrowserCookieName];
        }

        public static void ClearBrowserId(this HttpContext context)
        {
            if (context.Request.Cookies[WebConstants.AnonymounsBrowserCookieName] != null)
            {
                context.Response.Cookies.Delete(WebConstants.AnonymounsBrowserCookieName);
            }
        }
    }
}
