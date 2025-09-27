namespace Lifenix.API.Services
{
    using System;
    using Lifenix.API.Services.Interfaces;
    using Microsoft.AspNetCore.Http;

    public class CookieService : ICookieService
    {
        private readonly string jwtTokenKey = "jwt";

        // TODO: Make JWT cookie options configurable via appsettings.json
        public void SetJwtCookie(HttpResponse response, string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(24),
            };

            response.Cookies.Append(this.jwtTokenKey, token, cookieOptions);
        }

        public void RemoveJwtCookie(HttpResponse response)
        {
           response.Cookies.Delete(this.jwtTokenKey, new CookieOptions
           {
               Path = "/",
               Secure = true,
               SameSite = SameSiteMode.None,
           });
        }
    }
}
