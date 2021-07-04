

namespace MovieMania.Infrastructure.Domains
{
    public class Rating
    {
        public string Id { get; set; }
        public int Rate { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public Movie Movie { get; set; }
        public string MovieId { get; set; }
    }
}
