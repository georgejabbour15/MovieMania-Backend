using MovieMania.Infrastructure.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieMania.Contracts.MovieService
{
    public class GetAllMoviesResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Descritpion { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string IMBDLink { get; set; }
        public int Rating { get; set; }
        public string IMBDId { get; set; }
        public string ThumbnailUrl { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Country> Countries  { get; set; }
    }

}
