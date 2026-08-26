
using CookApp.Application.FiltrationClasses;
using CookApp.Application.Interfaces.Repositories;
using CookApp.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace CookApp.Data.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        readonly ApplicationContext _context;
        public RecipeRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Recipe> CreateRecipeAsync(Recipe recipe, CancellationToken token)
        {
            _context.Add(recipe);
            await _context.SaveChangesAsync(token);
            return recipe;
        }

        public async Task DeleteRecipe(Recipe recipe, CancellationToken token)
        {
            recipe.IsDeleted = true;
            await _context.SaveChangesAsync(token);
        }

        public async Task<Recipe?> GetRecipeByIdAsync(int id, CancellationToken token)
        {
            return await _context.Recipes.FirstOrDefaultAsync(r => r.RecipeId == id, token);
        }

        public async Task<List<Recipe>> GetRecipes(Filter filterOptions, CancellationToken cancellationToken)
        {
            IQueryable<Recipe> orderedRecipies = OrderRecipes(_context.Recipes, filterOptions.OrderType);
            IQueryable<Recipe> filteredRecipies = FilterRecipes(orderedRecipies, filterOptions.FiltrationType, filterOptions.FiltrationData);
            IQueryable<Recipe> paginatedRecipies = Paginate(filteredRecipies, filterOptions.Page);
            List<Recipe> requestedRecipies = await paginatedRecipies.ToListAsync(cancellationToken);

            return requestedRecipies;
        }

        private IQueryable<Recipe> OrderRecipes(IQueryable<Recipe> recipes,
         FiltrationOrder orderType)
        {
            return orderType switch

            {
                FiltrationOrder.Default => recipes.OrderBy(r => r.RecipeId),
                FiltrationOrder.ByYear => recipes.OrderBy(r => r.CreatedAt),
                _ => recipes.OrderBy(r => r.RecipeId)
            };
        }

        private IQueryable<Recipe> FilterRecipes(IQueryable<Recipe> recipes,
        FiltrationFilter filterType, string? filterData)
        {
            switch (filterType)
            {
                case FiltrationFilter.Default:
                    {
                        return recipes;
                    }
                case FiltrationFilter.ByYear:
                    {
                        if (filterData == null)
                            throw new ArgumentNullException("Date can't be null.");

                        DateOnly startDate = new(int.Parse(filterData), 1, 1);
                        DateOnly endDate = startDate.AddYears(1);
                        return recipes.Where(r => r.CreatedAt >= startDate && r.CreatedAt <= endDate);
                    }
                default:
                    {
                        goto case FiltrationFilter.Default;
                    }
            }
        }

        private IQueryable<Recipe> Paginate(IQueryable<Recipe> recipes, int pageNum)
        {
            if (pageNum < 1)
                throw new ArgumentException("Page num can't be less than 1.");

            return recipes.Skip((pageNum - 1) * 10).Take(10);
        }

    }
}