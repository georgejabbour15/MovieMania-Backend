
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MovieMania.AuthHandler;
using MovieMania.Data.DataContext;
using MovieMania.Infrastructure.Domains;
using MovieMania.Installers;
using System;
using System.Text;

namespace Deliver.Api.Installers
{
    public class AuthInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            //For Identity
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Secret"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = configuration["JWT:ValidIssuer"],
                ValidAudience=configuration["JWT:ValidAudience"],
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            //Adding Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CustomAuthConstants.UserAuthScheme;
            }).AddJwtBearer(x=>x.TokenValidationParameters=tokenValidationParameters)
             .AddScheme<CustomAuthOptions, CustomAuthHandler>
                     (CustomAuthConstants.UserAuthScheme, op => { });

            services.AddSingleton(tokenValidationParameters);
        }

    }
}
