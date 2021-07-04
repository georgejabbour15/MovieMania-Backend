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
    public static class MovieSeeder
    {
        public static async void Seed(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;
            var context = services.GetService<ApplicationDbContext>();


            if ( context.Movies.Count() == 0)
            {
                var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"data.json"}");
                var JSON = File.ReadAllText(folderDetails);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(JSON);
                JObject data = JObject.Parse(JSON);

                var x = data.Count;

                for (int i = 0; i < data.Count; i++)
                {
                    var item = data.GetValue(i.ToString());
                    Movie movie = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = (string)item["Name"],
                        Descritpion = (string)item["Description"],
                        ReleaseDate = (DateTime)item["ReleaseDate"],
                        IMBDLink = (string)item["IMDBLink"],
                        IMBDId = (string)item["IMDBId"],
                        ThumbnailUrl = (string)item["ThumbnailUrl"]

                    };
                    if (!context.Movies.Where(m => m.Name == movie.Name).Any())
                    {
                        await context.Movies.AddAsync(movie);
                        await context.SaveChangesAsync();

                        List<string> arrayGenres = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(item["Genres"].ToString());
                        foreach (var v in arrayGenres)
                        {
                            Genre genre = context.Genres.Where(g => g.Name == (string)v).FirstOrDefault();
                                MovieGenre movieGenre = new()
                                {
                                    MovieId =movie.Id,
                                    GenreId = genre.Id,
                                };
                                await context.MovieGenres.AddAsync(movieGenre);
                                await context.SaveChangesAsync();

                        }
                        List<string> arrayCountries = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(item["Countries"].ToString());
                        foreach (var v in arrayCountries)
                        {
                            Country country = context.Countries.Where(c => c.Name == (string)v).FirstOrDefault();
                            MovieCountry movieCountry = new()
                            {
                                MovieId = movie.Id,
                                CountryId = country.Id,
                            };
                            await context.MovieCountries.AddAsync(movieCountry);
                            await context.SaveChangesAsync();

                        }



                    }
                }
            }
        }
    }
}
