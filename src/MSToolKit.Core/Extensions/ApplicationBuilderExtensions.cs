using Microsoft.AspNetCore.Builder;
using MSToolKit.Core.Filters;
using MSToolKit.Core.Filters.Options;
using System;

namespace MSToolKit.Core.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseLoggingExceptionHandler(
            this IApplicationBuilder app, Action<LoggingExceptionHandlerOptions> exceptionOptions)
        {
            var options = new LoggingExceptionHandlerOptions();
            exceptionOptions(options);

            app.UseMiddleware<LoggingExceptionHandler>(options);

            return app;
        }
    }
}
