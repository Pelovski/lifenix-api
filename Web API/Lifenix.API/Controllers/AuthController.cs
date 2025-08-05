using Lifenix.API.DTOs.AuthDTOs;
using Lifenix.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lifenix.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;      
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login ([FromBody] LoginDto loginDto)
        {
            var result = await this.authService.LoginAsync(loginDto);

            if (!result.IsAuthSuccessful)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto) 
        {
            var result = await this.authService.RegisterAsync(registerDto);

            if (!result.IsAuthSuccessful)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }
    }
}
