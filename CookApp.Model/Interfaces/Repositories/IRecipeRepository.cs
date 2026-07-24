using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookApp.Model.Interfaces
{
    public interface IRecipeRepository
    {
        public IQueryable<Recipe> GetRecipes();

        public Task<List<T>> ToListAsync<T>(IQueryable<T> query);

    }
}