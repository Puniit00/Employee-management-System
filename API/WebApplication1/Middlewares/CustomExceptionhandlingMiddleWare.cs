using System.Text.Json;

namespace EmployeeManagementsystem.Middlewares
{
    public class CustomExceptionhandlingMiddleWare
    {
        public readonly RequestDelegate _next;
        public ILogger<CustomExceptionhandlingMiddleWare> _logger;
        public CustomExceptionhandlingMiddleWare(RequestDelegate next, ILogger<CustomExceptionhandlingMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        public Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            if (ex is ArgumentException)
            {
                statusCode = StatusCodes.Status400BadRequest;
            }
            else if(ex is UnauthorizedAccessException)
            {
                statusCode = StatusCodes.Status401Unauthorized;
            }
            else if (ex is KeyNotFoundException)
            {
                statusCode = StatusCodes.Status404NotFound;
            }
            var response = new
            {
                message = ex.Message,
                details = ex.InnerException?.Message,
                statusCode
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));

        }
    }
}
