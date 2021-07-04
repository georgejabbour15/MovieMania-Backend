using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MovieMania.Filters.Swagger
{
    public class PopulateVersionsInPath : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            //betjib kel l swaggDoc mnel startup w emshe biyon bi for loop
            var paths = new OpenApiPaths();
            foreach (var path in swaggerDoc.Paths)
                paths.Add(path.Key.Replace("v{version}", swaggerDoc.Info.Version), path.Value);

            swaggerDoc.Paths = paths;
        }
    }
}
