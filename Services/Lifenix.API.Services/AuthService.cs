namespace Lifenix.API.Services
{
    using System.Threading.Tasks;
    using Lifenix.API.DTOs.AuthDTOs;
    using Lifenix.API.Services.Interfaces;

    public class AuthService : IAuthService
    {
        public Task<AuthResponseDto> LoginAsync(LoginDto model)
        {
            throw new System.NotImplementedException();
        }

        public Task<AuthResponseDto> RegisterAsync(RegisterDto model)
        {
            throw new System.NotImplementedException();
        }
    }
}
