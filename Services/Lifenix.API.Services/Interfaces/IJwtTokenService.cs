namespace Lifenix.API.Services.Interfaces
{
    using System.Collections.Generic;
    using Lifenix.API.Data.Models;

    public interface IJwtTokenService
    {
        string GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    }
}
