using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagementsystem.Handlers
{
    public class CorrelationIdHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string HeaderKey = "X-Correlation-ID";

        public CorrelationIdHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var correlationId = _httpContextAccessor.HttpContext?.Request?.Headers[HeaderKey].ToString();
            if (!string.IsNullOrWhiteSpace(correlationId))
            {
                if (request.Headers.Contains(HeaderKey))
                    request.Headers.Remove(HeaderKey);

                request.Headers.Add(HeaderKey, correlationId);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
