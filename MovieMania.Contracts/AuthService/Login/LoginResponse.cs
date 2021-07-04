
using MovieMania.Infrastructure.Domains;
using System.Collections.Generic;


namespace MovieMania.Contracts.AuthService.Login
{
    public class LoginResponse
    {
        public User User { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
