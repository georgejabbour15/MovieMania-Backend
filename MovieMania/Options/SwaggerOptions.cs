using System.Collections.Generic;

namespace MovieMania.Options
{
    public class SwaggerOptions
    {
        public string Title { get; set; }
        public string Route { get; set; }
        public string Description { get; set; }
        public List<string> Versions { get; set; }
    }
}
