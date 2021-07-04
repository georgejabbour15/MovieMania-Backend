using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MovieMania.Data.DataContext;
using MovieMania.Infrastructure.Domains;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovieMania.Seed.DataSeeding
{
    public static class CountrySeeder
    {
        public static async void Seed(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;
            var context = services.GetService<ApplicationDbContext>();

            if (context.Countries.Count() == 0)
            {
                var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"data.json"}");
                var JSON = File.ReadAllText(folderDetails);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(JSON);
                JObject data = JObject.Parse(JSON);

                var x = data.Count;

                for (int i = 0; i < data.Count; i++)
                {
                    var item = data.GetValue(i.ToString());

                    List<string> arrayCountries = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(item["Countries"].ToString());
                    foreach (var v in arrayCountries)
                    {

                        if (!context.Countries.Where(c => c.Name == (string)v).Any())
                        {
                            Country countries = new()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = (string)v,
                            };
                            await context.Countries.AddAsync(countries);
                            await context.SaveChangesAsync();
                        }

                    }
                }

            }
        }
    }
}

