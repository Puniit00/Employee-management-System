using Dapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Constants;
using WebApplication1.Data;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IDbHelper _dbHelper;

        public EmployeesController(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // GET: api/employees
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                var employees = await _dbHelper.ExecuteTableAsync<Employee>(EmployeeConstants.StoreProcedures.GetAllEmployees, dynamicParameters);
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
