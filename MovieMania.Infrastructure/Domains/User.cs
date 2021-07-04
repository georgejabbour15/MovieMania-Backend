using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;


namespace MovieMania.Infrastructure.Domains
{
    public class User : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public List<Rating> Ratings  { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }

    }
}
