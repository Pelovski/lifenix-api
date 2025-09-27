namespace Lifenix.API.Services.Interfaces
{
    using Lifenix.API.DTOs.AuthDTOs;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto model);

        Task<AuthResponseDto> LoginAsync(LoginDto model);

        Task<string> GetJwtForExternalUserAsync(ClaimsPrincipal externalUser);
    }
}
