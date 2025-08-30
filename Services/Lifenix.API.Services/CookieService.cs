namespace Lifenix.API.Services
{
    using System;
    using Lifenix.API.Services.Interfaces;
    using Microsoft.AspNetCore.Http;

    public class CookieService : ICookieService
    {
        // TODO: Make JWT cookie options configurable via appsettings.json
        public void SetJwtCookie(HttpResponse response, string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(24),
            };

            response.Cookies.Append("jwt", token, cookieOptions);
        }
    }
}
