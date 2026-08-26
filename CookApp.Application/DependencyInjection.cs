using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CookApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IRecipeService, RecipeService>();
            services.AddSingleton<CustomCache>();
            return services;
        }
    }
}