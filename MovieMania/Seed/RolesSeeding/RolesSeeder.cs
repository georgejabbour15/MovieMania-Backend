using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MovieMania.Contracts.Enums;
using System;
using System.Threading.Tasks;

namespace Deliver.Api.Seed
{
    public static class RolesSeeder
    {       
        public static async void Seed(this IApplicationBuilder app)
        {

            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

                RoleManager<IdentityRole>roleManager = services.GetService<RoleManager<IdentityRole>>();
                try
                {
                if (!await roleManager.RoleExistsAsync(UserRolesEnums.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRolesEnums.User));
               
            }
                catch (TaskCanceledException e) {
                    Console.WriteLine($"{e}");
                }
        }
   
    }
}

