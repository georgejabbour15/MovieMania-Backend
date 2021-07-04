
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using MovieMania.Options;

namespace MovieMania.Extensions
{
    public static class SwaggerExtension
    {
        public static void SwaggerCaller(this IApplicationBuilder app, IConfiguration _configuration)
        {
            app.UseSwagger();

            SwaggerOptions swaggerOptions = _configuration.GetSection("SwaggerOptions").Get<SwaggerOptions>();

            app.UseSwaggerUI(c =>
            {
                foreach (var version in swaggerOptions.Versions)
                {
                    c.SwaggerEndpoint($"/swagger/v{version}/swagger.json", $"v{version}");
                }
            });
        }
    }
}
