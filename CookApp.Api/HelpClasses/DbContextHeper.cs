using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CookApp.Data;
using CookApp.Model;
using CookApp.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace CookApp.Api.HelpClasses
{
    public static class DbContextHeper
    {
        public async static void MigrateDb(WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();

            ApplicationContext context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.MigrateAsync();
            }

        }

        public async static void SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();

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
    }
}