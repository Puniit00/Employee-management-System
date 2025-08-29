using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebApplication1.Constants;
using WebApplication1.Data;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IDbHelper _dbHelper;
        private readonly IMemoryCache _cache;

        public EmployeesController(IDbHelper dbHelper, IMemoryCache cache)
        {
            _dbHelper = dbHelper;
            _cache = cache;
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding employee: {ex.Message}");
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error getting employee: {ex.Message}");

            }
        }
    }
}
