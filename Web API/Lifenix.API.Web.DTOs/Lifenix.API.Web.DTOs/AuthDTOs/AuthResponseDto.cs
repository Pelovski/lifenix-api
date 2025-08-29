namespace Lifenix.API.DTOs.AuthDTOs
{
    using System.Collections.Generic;

    public class AuthResponseDto
    {
        public bool IsAuthSuccessful { get; set; }

        public string Token { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
