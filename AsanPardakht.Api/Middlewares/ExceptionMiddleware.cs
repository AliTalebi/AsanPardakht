using AsanPardakht.Core.Errors;
using AsanPardakht.Core.Resources;

namespace AsanPardakht.Api.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger _exceptionLogger;
        private readonly IResourceManager _resourceManager;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IResourceManager resourceManager)
        {
            _exceptionLogger = logger;
            _resourceManager = resourceManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                string? unhandledMessage = _resourceManager[ApplicationErrors.Unhandled.Message];

                _exceptionLogger.LogError(exception: ex, message: unhandledMessage);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(new { Message = unhandledMessage });
            }
        }
    }
}
