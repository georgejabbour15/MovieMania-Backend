using System.ComponentModel.DataAnnotations;

namespace MovieMania.Contracts.AuthService.RefreshToken
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
