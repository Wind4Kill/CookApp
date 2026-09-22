using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Application.Authentication;
using CookApp.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CookApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRecipeService, RecipeService>();
            services.Configure<JwtTokenSettings>(configuration.GetSection("JwtSettings"));
            return services;
        }
    }
}