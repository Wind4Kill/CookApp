using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CookApp.Data;
using CookApp.Model;
using CookApp.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CookApp.Api.HelpClasses
{
    public static class DbContextHeper
    {
        public async static Task MigrateDb(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();

            ApplicationContext context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.MigrateAsync();
            }

        }

        public async static Task SeedData(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();

            ApplicationContext context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            if (!context.Recipes.Any())
            {
                Ingredient butter = new() { IngredientName = "Butter" };
                Ingredient chocolate = new() { IngredientName = "Chocolate" };
                Ingredient milk = new() { IngredientName = "Milk" };
                Ingredient cookies = new() { IngredientName = "Cookies" };
                Recipe recipe = new Recipe()
                {
                    RecipeName = "Chocolate sausage",
                    Ingredients = new List<Ingredient>() { butter, chocolate, milk, cookies }
                };

                context.Recipes.Add(recipe);
                await context.SaveChangesAsync();
            }
        }

        public static async Task AddAdmin(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            User? admin = await userManager.FindByEmailAsync(app.Configuration["AdminCredentials:Email"]!);
            if (admin is null)
            {
                admin = new(app.Configuration["AdminCredentials:Login"]!)
                {
                    Email = app.Configuration["AdminCredentials:Email"]!,
                };

                List<Claim> adminClaims = new()
                {
                    new Claim("Role", "Admin")
                };

                await userManager.CreateAsync(admin, app.Configuration["AdminCredentials:Password"]!);
                await userManager.AddClaimsAsync(admin, adminClaims);
            }

            return;

        }
    }
}