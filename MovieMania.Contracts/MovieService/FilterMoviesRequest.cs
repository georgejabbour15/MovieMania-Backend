using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieMania.Contracts.MovieService
{
    public class FilterMoviesRequest
    {
        public List<string> Genres { get; set; }
        public List<string> Countries { get; set; }
    }
}
