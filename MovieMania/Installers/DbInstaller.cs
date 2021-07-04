
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieMania.Data.DataContext;

namespace MovieMania.Installers
{
    public class DbInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            //Entity Framework : enables developers to work with a database using.NET objects
            services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("ConnStr"), x => x.MigrationsAssembly("MovieMania")));
        }
    }
}
