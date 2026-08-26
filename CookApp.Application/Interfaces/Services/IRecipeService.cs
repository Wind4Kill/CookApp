
using CookApp.Application.DTOs.RecipeDTOs;
using CookApp.Application.FiltrationClasses;

namespace CookApp.Application.Interfaces.Services
{
    public interface IRecipeService
    {
        public Task<List<GetRecipeDTO>> GetRecipes(Filter filterOptions, CancellationToken token);

        public Task<GetRecipeByIdDTO> GetRecipeById(int id, CancellationToken token);

        public Task<GetRecipeByIdDTO> CreateRecipe(CreateRecipeDTO recipeDTO, CancellationToken token);

        public Task DeleteRecipe(int id, CancellationToken token);
    }
}