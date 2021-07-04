using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MovieMania.Installers
{
    public interface IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration);
    }
}
