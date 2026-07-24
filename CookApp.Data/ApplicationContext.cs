
using CookApp.Model;
using CookApp.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace CookApp.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Recipe>? Recipes { get; set; } = null!;

        public DbSet<Ingredient>? Ingredients { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    }
}