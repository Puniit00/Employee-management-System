using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebApplication1.Constants;
using WebApplication1.Data;
using WebApplication1.Model;
using System.Net.Http.Json;
using EmployeeManagementsystem.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IDbHelper _dbHelper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<EmployeesController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;

        public EmployeesController(IDbHelper dbHelper, IMemoryCache cache, ILogger<EmployeesController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration, INotificationService notificationService)
        {
            _dbHelper = dbHelper;
            _cache = cache;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _notificationService = notificationService;
        }

        // GET: api/employees
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            string cacheKey = "employeesList";
            try
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                if(_cache.TryGetValue(cacheKey, out List<Employee> cachedEmployees))
                {
                    return Ok(cachedEmployees);
                }
                var employees = await _dbHelper.ExecuteTableAsync<Employee>(EmployeeConstants.StoreProcedures.GetAllEmployees, dynamicParameters);
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(2))
                    .SetPriority(CacheItemPriority.Normal);
                _cache.Set(cacheKey, employees, cacheEntryOptions);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employees");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving employees: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            try
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("name", employee.Name);
                dynamicParameters.Add("email", employee.Email);
                dynamicParameters.Add("department", employee.Department);
                dynamicParameters.Add("dateOfJoining", employee.DateOfJoining);

                var result = await _dbHelper.ExecuteScalarAsync<int>(
                    EmployeeConstants.StoreProcedures.AddEmployee, dynamicParameters
                );
                return CreatedAtAction(nameof(GetEmployees), new { id = result }, employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding employee");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding employee: {ex.Message}");
            }
        }

        // New endpoint: insert locally, notify other app, then log after remote confirms
        [HttpPost("add-and-notify")]
        public async Task<IActionResult> AddEmployeeAndNotify([FromBody] Employee employee)
        {
            try
            {
                _logger.LogInformation("Entering AddEmployeeAndNotify for {Email}", employee.Email);

                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("name", employee.Name);
                dynamicParameters.Add("email", employee.Email);
                dynamicParameters.Add("department", employee.Department);
                dynamicParameters.Add("dateOfJoining", employee.DateOfJoining);

                var newId = await _dbHelper.ExecuteScalarAsync<int>(
                    EmployeeConstants.StoreProcedures.AddEmployee, dynamicParameters
                );

                _logger.LogInformation("Inserted employee locally with id {EmployeeId}", newId);

                var client = _httpClientFactory.CreateClient("ApiToApiClient");
                var endpoint = _configuration["OtherApi:InsertLogEndpoint"];

                // determine correlation id to include in notification
                var correlationId = HttpContext.Request.Headers.TryGetValue("X-Correlation-ID", out var cid) && !string.IsNullOrWhiteSpace(cid) ? cid.ToString() : HttpContext.TraceIdentifier;

                if (!string.IsNullOrEmpty(endpoint))
                {
                    var payload = new { employeeId = newId, name = employee.Name, email = employee.Email };
                    var resp = await client.PostAsJsonAsync(endpoint, payload);
                    if (resp.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Remote app confirmed log insertion for employee {EmployeeId}", newId);

                        // broadcast notification to connected SignalR clients including correlation id
                        await _notificationService.NotifyEmployeeCreated(newId, employee.Name, employee.Email, correlationId);

                        _logger.LogInformation("Notification sent for employee {EmployeeId}", newId);
                    }
                    else
                    {
                        _logger.LogWarning("Remote app did not confirm log insertion. Status: {StatusCode}", resp.StatusCode);
                    }
                }
                else
                {
                    _logger.LogWarning("OtherApi endpoint not configured.");
                }

                _logger.LogInformation("Finalizing AddEmployeeAndNotify for {EmployeeId}", newId);

                return CreatedAtAction(nameof(GetEmployees), new { id = newId }, employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddEmployeeAndNotify");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding employee and notifying remote: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            try
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("id", id);
                dynamicParameters.Add("name", employee.Name);
                dynamicParameters.Add("email", employee.Email);
                dynamicParameters.Add("department", employee.Department);
                dynamicParameters.Add("dateOfJoining", employee.DateOfJoining);
                var result = await _dbHelper.ExecuteScalarAsync<int>(
                    EmployeeConstants.StoreProcedures.UpdateEmployee, dynamicParameters
                );
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating employee: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("id", id);
                var result = await _dbHelper.ExecuteScalarAsync<int>(
                    EmployeeConstants.StoreProcedures.DeleteEmployee, dynamicParameters
                );
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting employee: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("id", id);
                var result = await _dbHelper.ExecuteTableAsync<Employee>(
                    EmployeeConstants.StoreProcedures.GetEmployeeById, dynamicParameters
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employee");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error getting employee: {ex.Message}");

            }
        }
    }
}
