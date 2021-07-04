
using System.ComponentModel.DataAnnotations;

namespace MovieMania.Contracts.AuthService.Login
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
