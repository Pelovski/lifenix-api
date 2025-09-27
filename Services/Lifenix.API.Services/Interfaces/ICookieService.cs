namespace Lifenix.API.Services.Interfaces
{
    using Microsoft.AspNetCore.Http;

    public interface ICookieService
    {
        void SetJwtCookie(HttpResponse response, string token);

        void RemoveJwtCookie(HttpResponse response);
    }
}
