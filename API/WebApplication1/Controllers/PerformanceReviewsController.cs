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
    public class PerformanceReviewsController : Controller
    {
        private readonly IDbHelper _dbHelper;
        public PerformanceReviewsController(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        [HttpPost]
        [Route("AddPerformanceReview")]
        public async Task<IActionResult> AddPerformanceReview([FromBody] PerformanceReview performanceReview)
        {
            try
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("employeeId", performanceReview.Id);
                dynamicParameters.Add("reviewerName", performanceReview.ReviewerName);
                dynamicParameters.Add("reviewDate", performanceReview.ReviewDate);
                dynamicParameters.Add("rating", performanceReview.Rating);
                dynamicParameters.Add("comments", performanceReview.Comments);
                var result = await _dbHelper.ExecuteScalarAsync<int>(
                    EmployeeConstants.StoreProcedures.AddPerformanceReview, dynamicParameters
                );
                return CreatedAtAction(nameof(GetPerformanceReviews), new { employeeId = result }, performanceReview);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding performance reviews: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetPerformanceReviews/{employeeId}")]
        public async Task<IActionResult> GetPerformanceReviews(int employeeId)
        {
            try
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("employeeId", employeeId);
                var performanceReviews = await _dbHelper.ExecuteTableAsync<PerformanceReview>(
                    EmployeeConstants.StoreProcedures.GetPerformanceReviews, dynamicParameters
                );
                return Ok(performanceReviews);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving performance Reviews: {ex.Message}");
            }
        }
    }
}
