using Newtonsoft.Json;
using System.Net;

namespace Tours.API.Middleware
{
    public class MiddlewareHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MiddlewareHandler> _logger;

        public MiddlewareHandler(RequestDelegate next, ILogger<MiddlewareHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            switch (exception)
            {
                case InvalidCredentialsException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        error = exception.Message,
                    }));

                case EmailAlreadyExistsException:
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    return context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        error = exception.Message,
                    }));

                default:
                    _logger.LogError(exception, "An unexpected error occurred");
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        error = exception.Message,
                    }));
            }
        }
    }
}
