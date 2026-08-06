
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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var createdRecipies = ChangeTracker.Entries<Recipe>().
            Where(r => r.State == EntityState.Added).ToList();

            foreach(var entry in createdRecipies)
            {
                entry.Property<DateOnly>("CreatedAt").CurrentValue = DateOnly.FromDateTime(DateTime.Now);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}