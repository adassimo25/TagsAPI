using Microsoft.AspNetCore.Diagnostics;

namespace TagsAPI.Exceptions
{
    public class AppExceptionHandler(ILogger<AppExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<AppExceptionHandler> logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var message = exception.Message;
            var stackTrace = exception.StackTrace;

            (int statusCode, string errorMessage, string? errorStackTrace) = exception switch
            {
                ArgumentException => (StatusCodes.Status400BadRequest, message, null),
                ExternalAPIException => (StatusCodes.Status500InternalServerError, message, stackTrace),
                _ => (StatusCodes.Status500InternalServerError, $"Something went wrong! {message}", null)
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "text/plain";

            await httpContext.Response.WriteAsync(errorMessage, cancellationToken);

            logger.LogError(errorMessage);
            logger.LogError(errorStackTrace);

            return true;
        }
    }
}
