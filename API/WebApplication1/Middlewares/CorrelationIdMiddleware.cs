using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace EmployeeManagementsystem.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string HeaderKey = "X-Correlation-ID";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(HeaderKey, out var correlationId) || string.IsNullOrWhiteSpace(correlationId))
            {
                correlationId = Activity.Current?.Id ?? Guid.NewGuid().ToString();
                context.Request.Headers[HeaderKey] = correlationId;
            }

            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey(HeaderKey))
                {
                    context.Response.Headers.Add(HeaderKey, correlationId.ToString());
                }
                return Task.CompletedTask;
            });

            using (LogContext.PushProperty("CorrelationId", correlationId.ToString()))
            {
                await _next(context);
            }
        }
    }
}
