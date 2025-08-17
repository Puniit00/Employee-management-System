using Dapper;
using EmployeeManagementsystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using WebApplication1.Constants;
using WebApplication1.Data;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IDbHelper _dbHelper;

    public AuthController(IConfiguration config, IDbHelper dbHelper)
    {
        _config = config;
        _dbHelper = dbHelper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        Users userData = await this.GetUserData(model);
        if (userData != null)
        {
            var token = GenerateJwtToken(model.Username);
            return Ok(new { Token = token });
        }
        return Unauthorized();
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Create(int badgeNo, string name, string password)
    {
        if (password == null || badgeNo <= 0 || name == null)
            return BadRequest("Invalid user data");

        var newUserId = await this.CreateUserAsync(badgeNo, name, password);

        return Ok(new { Message = "User created successfully", UserId = newUserId });
    }

    private string GenerateJwtToken(string username)
    {
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<Users> GetUserData(LoginModel login)
    {
        DynamicParameters paramters = new();
        paramters.Add("username", login.Username);
        paramters.Add("password", login.Password);
        var user = await _dbHelper.ExecuteTableAsync<Users>(
            EmployeeConstants.StoreProcedures.GetUserByUsernameAndPassword, paramters, CommandType.StoredProcedure);
        return (user[0] as Users);
    }

    private async Task<int> CreateUserAsync(int badgeNo, string name, string password)
    {
        DynamicParameters parameters = new();
        parameters.Add("badgeNo", badgeNo);
        parameters.Add("name", name);
        parameters.Add("password", password);
        var userId = await _dbHelper.ExecuteScalarAsync<int>(
            EmployeeConstants.StoreProcedures.CreateUser, parameters, CommandType.StoredProcedure);
        return userId;
    }

}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}
