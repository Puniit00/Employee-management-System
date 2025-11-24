using Microsoft.AspNetCore.SignalR;

namespace EmployeeManagementsystem.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            var http = Context.GetHttpContext();
            var correlationId = http?.Request.Headers["X-Correlation-ID"].ToString();
            if (string.IsNullOrWhiteSpace(correlationId))
            {
                correlationId = http?.Request.Query["correlationId"].ToString();
            }

            _logger.LogInformation("Client connected. ConnectionId: {ConnectionId}, CorrelationId: {CorrelationId}", Context.ConnectionId, string.IsNullOrWhiteSpace(correlationId) ? "n/a" : correlationId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("Client disconnected. ConnectionId: {ConnectionId}, Reason: {Reason}", Context.ConnectionId, exception?.Message ?? "client disconnected");
            return base.OnDisconnectedAsync(exception);
        }

        // Clients can call this to acknowledge a notification was processed
        public Task AcknowledgeEmployeeCreated(int employeeId, string correlationId)
        {
            _logger.LogInformation("Client {ConnectionId} acknowledged EmployeeCreated for {EmployeeId} with CorrelationId {CorrelationId}", Context.ConnectionId, employeeId, correlationId);
            return Task.CompletedTask;
        }

        // Allow clients to join named groups (e.g., per-user or per-tenant)
        public Task JoinGroup(string groupName)
        {
            _logger.LogInformation("Client {ConnectionId} joining group {Group}", Context.ConnectionId, groupName);
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public Task LeaveGroup(string groupName)
        {
            _logger.LogInformation("Client {ConnectionId} leaving group {Group}", Context.ConnectionId, groupName);
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
