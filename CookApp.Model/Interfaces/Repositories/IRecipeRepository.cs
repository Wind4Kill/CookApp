using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Model.DTOs;
using CookApp.Model.DTOs.RecipeDTOs;

namespace CookApp.Model.Interfaces
{
    public interface IRecipeRepository
    {
        public IQueryable<Recipe> GetRecipes();
        public Task<Recipe?> GetRecipeByIdAsync(int id);
        public Task<List<T>> ToListAsync<T>(IQueryable<T> query);

        public Task<Recipe> CreateRecipeAsync(Recipe recipe);

        public Task<int> DeleteRecipe(Recipe recipe);
    }
}