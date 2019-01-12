using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MSToolKit.Core.Filters.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MSToolKit.Core.Filters
{
    /// <summary>
    /// Provides functionality for exception handling.
    /// </summary>
    internal class LoggingExceptionHandler
    {
        private readonly RequestDelegate next;
        private readonly ILogger<LoggingExceptionHandler> logger;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly LoggingExceptionHandlerOptions options;


        /// <summary>
        /// Initializes a new instance of MSToolKit.Core.Filters.LoggingExceptionHandler.
        /// </summary>
        /// <param name="next">The function that process the current HTTP request.</param>
        /// <param name="logger">The instance for Microsoft.Extensions.Logging.ILogger, that will log the occured errors.</param>
        /// <param name="hostingEnvironment">The current instance of Microsoft.AspNetCore.Hosting.IHostingEnvironment.</param>
        /// <param name="options">
        /// An instance of MSToolKit.Core.Filters.Options.ExceptionHandlerOptions, 
        /// that configures the behavior of the exception handler.
        /// </param>
        public LoggingExceptionHandler(
            RequestDelegate next, 
            ILogger<LoggingExceptionHandler> logger,
            IHostingEnvironment hostingEnvironment,
            LoggingExceptionHandlerOptions options)
        {
            this.next = next;
            this.logger = logger;
            this.hostingEnvironment = hostingEnvironment;
            this.options = options;
        }

        /// <summary>
        /// Invokes the current request delegate and, 
        /// if any exception has thrown, handle it and log the occured error.
        /// After that rethrow the original exception or build a response, 
        /// depending on the ExceptionHandlerOptions of the current instance.
        /// </summary>
        /// <param name="context">
        /// The current instance of Microsoft.AspNetCore.Http.HttpContext, 
        /// that encapsulates all HTTP-specific information about an individual HTTP request.
        /// </param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation.
        /// </returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next.Invoke(context);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var error = this.BuildErrorMessage(exception);

            this.logger.LogError(error);

            var response = context.Response;
            if (this.hostingEnvironment.IsDevelopment())
            {
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await response.WriteAsync(error);
            }
            else
            {
                response.Redirect(this.options.LocalErrorUrl ?? "/");
            }
            
            if (this.options.ShouldRethrowException)
            {
                throw exception;
            }
        }

        private string BuildErrorMessage(Exception exception)
        {
            return JsonConvert.SerializeObject(new
            {
                error = new
                {
                    message = exception.Message,
                    exception = exception.GetType().Name,
                    stackTrace = exception
                        .StackTrace
                        .Split(Environment.NewLine)
                        .Select(r => r.Trim())
                }
            },
            Formatting.Indented);
        }
    }
}
