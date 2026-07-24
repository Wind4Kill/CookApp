using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Application;
using CookApp.Data.Repositories;
using CookApp.Model.Interfaces;
using CookApp.Model.Interfaces.Services;

namespace CookApp.Api.HelpClasses
{
    public static class ServiceHelper
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IRecipeService, RecipeService>();
            return services;
        }
    }
}