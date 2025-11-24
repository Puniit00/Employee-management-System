using Microsoft.AspNetCore.SignalR;
using EmployeeManagementsystem.Hubs;

namespace EmployeeManagementsystem.Services
{
    public interface INotificationService
    {
        Task NotifyEmployeeCreated(int employeeId, string name, string email, string correlationId);
    }

    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyEmployeeCreated(int employeeId, string name, string email, string correlationId)
        {
            var payload = new { employeeId, name, email, correlationId };
            await _hubContext.Clients.All.SendAsync("EmployeeCreated", payload);
        }
    }
}
