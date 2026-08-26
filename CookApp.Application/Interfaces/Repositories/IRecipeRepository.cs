using System;
using System.Collections.Generic;
using System.Linq;
using CookApp.Application.FiltrationClasses;
using CookApp.Model.Entities;

namespace CookApp.Application.Interfaces.Repositories
{
    public interface IRecipeRepository
    {
        public Task<List<Recipe>> GetRecipes(Filter filerOptions, CancellationToken cancellationToken);
        public Task<Recipe?> GetRecipeByIdAsync(int id, CancellationToken token);
        public Task<Recipe> CreateRecipeAsync(Recipe recipe, CancellationToken token);

        public Task DeleteRecipe(Recipe recipe, CancellationToken token);
    }
}