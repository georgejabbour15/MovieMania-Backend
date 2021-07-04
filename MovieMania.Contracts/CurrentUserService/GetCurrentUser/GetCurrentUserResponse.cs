
using MovieMania.Infrastructure.Domains;
using System.Collections.Generic;


namespace MovieMania.Contracts.CurrentUserService.GetCurrentUser
{
    public class GetCurrentUserResponse
    {
        public User User { get; set; }
        public List<string> Role { get; set; }
    }
}
