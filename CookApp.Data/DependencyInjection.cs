using System.Diagnostics;
using CookApp.Application.Interfaces;
using CookApp.Application.Interfaces.Authentication;
using CookApp.Application.Interfaces.Caching;
using CookApp.Application.Interfaces.Repositories;
using CookApp.Data.Authentication;
using CookApp.Data.Caching;
using CookApp.Data.Repositories;
using CookApp.Model.Entities;
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

            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationContext>();

            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddSingleton(typeof(ICacheService<>), typeof(CacheService<>));
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<ITokenProvider, JwtTokenProvider>();
            
            return services;
        }
    }
}