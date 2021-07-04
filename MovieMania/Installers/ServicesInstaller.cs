using AuthService;
using CurrentUserService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieService;

namespace MovieMania.Installers
{
    public class ServicesInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService.AuthService>();
            services.AddScoped<ICurrentUserService, CurrentUserService.CurrentUserService>();
            services.AddScoped<IMovieService, MovieService.MovieService>();


        }
    }
}
