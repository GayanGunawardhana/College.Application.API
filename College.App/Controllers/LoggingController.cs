using College.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace College.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoggingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LoggingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public ActionResult Login(LoginRequestDTO model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Please provide username and password");
            }
            LoginResponseDTO loginResponseDTO = new() { Username=model.Username};
            string audience = string.Empty;
            string issuer = string.Empty;

            // Valid user
            audience = _configuration.GetValue<string>("Jwt:Audience");
            issuer = _configuration.GetValue<string>("Jwt:Issuer");
            // In a real application, you would generate a JWT token here
            // using the secret key from configuration settings.

            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JwtSettings")); // Getting the secret key from configuration

            if (model.Username == "admin" && model.Password == "admin123")
            {
                
                var tokenHandler = new JwtSecurityTokenHandler(); // JWT token handler
                var tokenDescriptor = new SecurityTokenDescriptor // Token descriptor
                {
                    // Setting audience and issuer
                    Audience = audience,
                    Issuer = issuer,

                    // Token generation logic would go here
                    Subject = new ClaimsIdentity(new[]
                    {
                        
                        // username claim   
                        new Claim(ClaimTypes.Name, model.Username),
                        // role claim
                        new Claim(ClaimTypes.Role, "Admin")
                    }),
                    Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Signing the token

                }; 
                var token = tokenHandler.CreateToken(tokenDescriptor); // Create the token
                loginResponseDTO.token = tokenHandler.WriteToken(token); // Write the token to string format


                return Ok(loginResponseDTO);
            }
            else
            {
                return NotFound("Invalid username and password");
            }


        }


    }
  
}
