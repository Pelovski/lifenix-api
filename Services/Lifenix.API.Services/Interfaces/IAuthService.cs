namespace Lifenix.API.Services.Interfaces
{
    using System.Threading.Tasks;
    using Lifenix.API.DTOs.AuthDTOs;

    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto model);

        Task<AuthResponseDto> LoginAsync(LoginDto model);
    }
}
