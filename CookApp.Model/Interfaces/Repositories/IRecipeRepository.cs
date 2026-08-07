using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Model.DTOs;
using CookApp.Model.DTOs.RecipeDTOs;
using CookApp.Model.FiltrationClasses;

namespace CookApp.Model.Interfaces
{
    public interface IRecipeRepository
    {
        public IQueryable<Recipe> GetRecipes();
        public Task<Recipe?> GetRecipeByIdAsync(int id, CancellationToken token);
        public Task<Recipe> CreateRecipeAsync(Recipe recipe, CancellationToken token);

        public Task DeleteRecipe(Recipe recipe, CancellationToken token);
    }
}