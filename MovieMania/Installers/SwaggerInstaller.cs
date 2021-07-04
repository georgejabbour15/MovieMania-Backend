
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MovieMania.Conventions;
using MovieMania.Filters.Swagger;
using MovieMania.Options;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace MovieMania.Installers
{
    public class SwaggerInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers(options =>
            {
                options.Conventions.Add(new GroupingByNamespaceConvention());
                options.EnableEndpointRouting = false;

            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            })
             .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = ApiVersion.Default;
                options.ReportApiVersions = true;
            });

            SwaggerOptions swaggerOptions = configuration.GetSection("SwaggerOptions").Get<SwaggerOptions>();

            //Implement swagger
            services.AddSwaggerGen(c =>
            {
                foreach (string v in swaggerOptions.Versions)
                {
                    c.SwaggerDoc($"v{v}", new OpenApiInfo
                    {
                        Version = $"v{v}",
                        Title = swaggerOptions.Title,
                        Description = swaggerOptions.Description
                    }
                    );
                }

                c.OperationFilter<RemoveVersionFromParameter>();
                c.DocumentFilter<PopulateVersionsInPath>();

                //Dropdown
                c.DocInclusionPredicate((version, description) =>
                {
                    if (!description.TryGetMethodInfo(out MethodInfo methodInfo))
                        return false;
                    //getting all the versions
                    var versions = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<ApiVersionAttribute>().SelectMany(attr => attr.Versions).ToArray();
                    var maps = methodInfo.GetCustomAttributes(true).OfType<MapToApiVersionAttribute>().SelectMany(attr => attr.Versions).ToArray();
                    return versions.Any(v => $"v{v}" == version) && (!maps.Any() || maps.Any(v => $"v{v}" == version));
                });

                //Bearer in swagger
                Dictionary<string, IEnumerable<string>> security = new Dictionary<string, IEnumerable<string>>
                  {
                    {"Bearer",new string[0]}
                  };
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWt Autorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                  {
                    {new OpenApiSecurityScheme{Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }},new List<string>() }
                  });

                //Swagger summary
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
        }
    }
}
