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

        public async Task<AuthResponseDto> LoginAsync(LoginDto model)
        {
            var user = await this.userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new AuthResponseDto
                {
                    IsAuthSuccessful = false,
                    Errors = new List<string> { "Invalid email or password." },
                };
            }

            var result = await this.signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);

            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    IsAuthSuccessful = false,
                    Errors = new List<string> { "Invalid email or password." },
                };
            }

            var roles = await this.userManager.GetRolesAsync(user);
            var token = this.jwtTokenService.GenerateToken(user, roles);

            return new AuthResponseDto
            {
                IsAuthSuccessful = true,
                Token = token,
                UserId = user.Id,
                Username = user.UserName,
                Roles = roles,
                Errors = null,
            };
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
