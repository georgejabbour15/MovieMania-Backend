

using System.Collections.Generic;

namespace MovieMania.Infrastructure.Domains
{
    public class Country
    {
        public string Id { get; set; }
        public string Name  { get; set; }
        public List<MovieCountry> MovieCountries { get; set; }
    }
}
