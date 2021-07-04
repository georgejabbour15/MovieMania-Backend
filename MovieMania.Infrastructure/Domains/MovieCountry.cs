using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieMania.Infrastructure.Domains
{
    public class MovieCountry
    {
        public string MovieId { get; set; }
        public Movie Movie { get; set; }
        public string CountryId { get; set; }
        public Country Country  { get; set; }
    }
}
