using System;
using System.Collections.Generic;

namespace MovieMania.Infrastructure.Domains
{
    public class Movie
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Descritpion { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string IMBDLink { get; set; }
        public string IMBDId { get; set; }
        public string ThumbnailUrl { get; set; }
      
        public List<Rating> Ratings { get; set; }
        public List<MovieGenre> MovieGenres  { get; set; }
        public List<MovieCountry> MovieCountries { get; set; }
    }
    
}
