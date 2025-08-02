namespace Lifenix.API.Services
{
    using System.Threading.Tasks;
    using Lifenix.API.Services.Interfaces;

    public class AuthService : IAuthService
    {
        public Task<string> LoginAsync(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> RegisterAsync(string username, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}
