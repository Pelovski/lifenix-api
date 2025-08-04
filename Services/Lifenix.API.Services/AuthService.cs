namespace Lifenix.API.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Lifenix.API.Data.Models;
    using Lifenix.API.DTOs.AuthDTOs;
    using Lifenix.API.Services.Interfaces;
    using Microsoft.AspNetCore.Identity;

    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IJwtTokenService jwtTokenService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenService jwtTokenService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtTokenService = jwtTokenService;
        }

        public Task<AuthResponseDto> LoginAsync(LoginDto model)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto model)
        {
            var existingEmail = await this.userManager.FindByEmailAsync(model.Email);
            var existingUsername = await this.userManager.FindByNameAsync(model.Username);

            if (existingEmail != null)
            {
                return new AuthResponseDto
                {
                    IsAuthSuccessful = false,
                    Errors = new List<string> { "Email is already registered." },
                };
            }

            if (existingUsername != null)
            {
                return new AuthResponseDto
                {
                    IsAuthSuccessful = false,
                    Errors = new List<string> { "Username is already taken." },
                };
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
            };

            var result = await this.userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();

                return new AuthResponseDto
                {
                    IsAuthSuccessful = false,
                    Errors = errors,
                };
            }

            await this.userManager.AddToRoleAsync(user, "User");

            var roles = await this.userManager.GetRolesAsync(user);
            var token = this.jwtTokenService.GenerateToken(user, roles);

            return new AuthResponseDto
            {
                IsAuthSuccessful = true,
                Token = token,
                UserId = user.Id,
                Username = model.Username,
                Roles = roles,
            };
        }
    }
}
