using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Constants;
using WebApplication1.Data;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Authorize]
    public class EmployeeGoalsController : Controller
    {
        private readonly IDbHelper _dbHelper;
        public EmployeeGoalsController(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        [HttpPost]
        [Route("AddEmployeeGoals")]
        public async Task<IActionResult> AddEmployeeGoals([FromBody] EmployeeGoals employeeGoals)
        {
            try
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("employeeId", employeeGoals.EmployeeId);
                dynamicParameters.Add("title", employeeGoals.Title);
                dynamicParameters.Add("targetDate", employeeGoals.TargetDate);
                dynamicParameters.Add("isComplete", employeeGoals.IsCompleted);
                dynamicParameters.Add("description", employeeGoals.Description);
                var result = await _dbHelper.ExecuteScalarAsync<int>(
                    EmployeeConstants.StoreProcedures.AddEmployeeGoals, dynamicParameters
                );
                return Ok(employeeGoals);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding employee goals: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetEmployeeGoals/{employeeId}")]
        public async Task<IActionResult> GetEmployeeGoals(int employeeId)
        {
            try
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("employeeId", employeeId);
                var goals = await _dbHelper.ExecuteTableAsync<EmployeeGoals>(
                    EmployeeConstants.StoreProcedures.GetEmployeeGoals, dynamicParameters
                );
                return Ok(goals);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving employee goals: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("UpdateEmployeeGoals/{id}")]
        public async Task<IActionResult> UpdateEmployeeGoals(int id,[FromBody] EmployeeGoals employeeGoals)
        {
            try
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("id", id);
                dynamicParameters.Add("title", employeeGoals.Title);
                dynamicParameters.Add("targetDate", employeeGoals.TargetDate);
                dynamicParameters.Add("isComplete", employeeGoals.IsCompleted);
                dynamicParameters.Add("description", employeeGoals.Description);
                
                var result = await _dbHelper.ExecuteScalarAsync<int>(
                    EmployeeConstants.StoreProcedures.UpdateEmployeeGoals, dynamicParameters
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating employee goals: {ex.Message}");
            }
        }
    }
}
