using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Model;
using CookApp.Model.DTOs.RecipeDTOs;
using CookApp.Model.FiltrationClasses;
using CookApp.Model.Interfaces;
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

        public IQueryable<Recipe> GetRecipes()
        {
            return _context.Recipes.AsNoTracking();
            
        }

    }
}