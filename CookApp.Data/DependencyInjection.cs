using System.Diagnostics;
using CookApp.Application.Interfaces.Repositories;
using CookApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CookApp.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddData(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                if (connectionString is null)
                {
                    throw new Exception("Data base connection string is empty.");
                }
                options.UseNpgsql(connectionString,
                options => options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(3), null))
                .LogTo((message) => Debug.WriteLine(message))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
            });

            services.AddScoped<IRecipeRepository, RecipeRepository>();
            
            return services;
        }
    }
}