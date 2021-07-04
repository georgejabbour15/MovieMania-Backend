
using System;


namespace MovieMania.Infrastructure.Domains
{
    public class RefreshToken
    {
        
        public string Id { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public bool KeepLogin { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
    }
}