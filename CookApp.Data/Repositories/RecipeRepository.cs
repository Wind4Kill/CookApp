using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Model;
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