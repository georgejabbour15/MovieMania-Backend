
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieMania.Installers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieMania.Extensions
{
    public static class InstallersExtension
    {
        //this : ya3ne InstallAllServices saret inclus bel services laken lezem 3ayetla "services.InstallAllServices"
        public static void InstallAllServices(this IServiceCollection services,IConfiguration configuration)
        {
            //typeof(startup)->class startup
            //Assembly ->Deliver.api
            //exportedtypes ->all classes kellon
            //typeof(IInstaller).IsAssignableFrom(x) : x is of type IInstaller??
            //!x.IsInterface && !x.IsAbstract : lezem l method tkoun implemented
            //.Select(Activator.CreateInstance) : 3am jib instance mn l class li 3meltella select

            List<IInstaller> installers = typeof(Startup).Assembly.ExportedTypes.Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            installers.ForEach(i => i.Install(services, configuration));    
        }
    }
}
