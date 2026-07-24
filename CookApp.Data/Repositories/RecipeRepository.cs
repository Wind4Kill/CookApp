using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Model;
using CookApp.Model.DTOs.RecipeDTOs;
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

        public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
        {
            _context.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }

        public async Task<Recipe?> GetRecipeByIdAsync(int id)
        {
            return await _context.Recipes.FirstOrDefaultAsync(r => r.RecipeId == id);
        }

        public IQueryable<Recipe> GetRecipes()
        {
            return _context.Recipes.AsNoTracking();
        }

        public async Task<List<T>> ToListAsync<T>(IQueryable<T> query)
        {
            return await query.ToListAsync();
        }
    }
}