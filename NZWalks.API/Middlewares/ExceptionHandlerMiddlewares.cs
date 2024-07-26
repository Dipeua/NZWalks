using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddlewares
    { 
        private readonly ILogger<ExceptionHandlerMiddlewares> _logger;
        private readonly RequestDelegate _next;
        public ExceptionHandlerMiddlewares(ILogger<ExceptionHandlerMiddlewares> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception ex)
            {
                var errorId = Guid.NewGuid();

                // Log this exception
                _logger.LogError(ex, $"{errorId}: {ex.Message}");

                // return a custome error response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong! We are looking into resolving this"
                };

                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
