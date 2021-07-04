using System.Collections.Generic;

namespace MovieMania.Infrastructure.Domains
{
    public class Genre
    { 
        public string Id { get; set; }
        public string Name   { get; set; }
        public List<MovieGenre> MovieGenres { get; set; }
    }
}
