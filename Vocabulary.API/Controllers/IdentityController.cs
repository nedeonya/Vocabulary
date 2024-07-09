using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Vocabulary.API.Dto;

namespace Vocabulary.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdentityController: Controller
{
    private readonly IConfiguration _config;
    public IdentityController( IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] UserDto user)
    {
        Claim[] claims = [new Claim(ClaimTypes.NameIdentifier, user.Id)];
        
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
            SecurityAlgorithms.HmacSha256);
            
        var token = new JwtSecurityToken (
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: signingCredentials);
        
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        
        return Ok(tokenValue);
    }
}