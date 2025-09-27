using Lifenix.API.DTOs.AuthDTOs;
using Lifenix.API.Services;
using Lifenix.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lifenix.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ICookieService cookieService;

        public AuthController(IAuthService authService, ICookieService cookieService)
        {
            this.authService = authService;
            this.cookieService = cookieService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login ([FromBody] LoginDto loginDto)
        {
            var result = await this.authService.LoginAsync(loginDto);

            if (!result.IsAuthSuccessful)
            {
                return BadRequest(result.Errors);
            }

            this.cookieService.SetJwtCookie(Response, result.Token);

            return Ok(result.IsAuthSuccessful);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto) 
        {
            var result = await this.authService.RegisterAsync(registerDto);

            if (!result.IsAuthSuccessful)
            {
                return BadRequest(result.Errors);
            }

            this.cookieService.SetJwtCookie(Response, result.Token);

            return Ok(result);
        }

        [HttpGet("status")]
        [Authorize]
        public IActionResult GetStatus()
        {
            return Ok();
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            this.cookieService.RemoveJwtCookie(Response);

            return Ok();
        }

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };

            return Challenge(properties, "Google");
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await this.HttpContext.AuthenticateAsync("Google");

            if (!result.Succeeded || result.Principal == null)
            {
                return BadRequest("Google authentication failed.");
            }

            var jwtToken = await this.authService.GetJwtForExternalUserAsync(result.Principal);
            this.cookieService.SetJwtCookie(Response, jwtToken);

            return this.Redirect("http://localhost:4200/dashboard/");
        }
    }
}
