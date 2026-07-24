
using System.Reflection;
using CookApp.Model;
using CookApp.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace CookApp.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}