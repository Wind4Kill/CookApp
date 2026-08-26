using AutoMapper;
using CookApp.Application.DTOs.RecipeDTOs;
using CookApp.Application.FiltrationClasses;
using CookApp.Application.Interfaces.Repositories;
using CookApp.Application.Interfaces.Services;
using CookApp.Model;
using CookApp.Model.Entities;
using CookApp.Model.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace CookApp.Application
{
    public class RecipeService : IRecipeService
    {
        readonly IRecipeRepository _recipeRepo;
        readonly IMapper _mapper;
        readonly CustomCache _cache;

        public RecipeService(IRecipeRepository recipeRepository, IMapper mapper, CustomCache cache)
        {
            _recipeRepo = recipeRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<GetRecipeByIdDTO> CreateRecipe(CreateRecipeDTO recipeDTO, CancellationToken token)
        {
            Recipe recipe = _mapper.Map<Recipe>(recipeDTO);

            recipe = await _recipeRepo.CreateRecipeAsync(recipe, token);

            return _mapper.Map<GetRecipeByIdDTO>(recipe);
        }

        public async Task DeleteRecipe(int id, CancellationToken token)
        {
            Recipe requestedRecipe = await CheckAndReturnRecipe(id, token);
            string key = GetKeyString(id);
            await _recipeRepo.DeleteRecipe(requestedRecipe, token);
            _cache.Cache.Remove(key);
        }

        public async Task<GetRecipeByIdDTO> GetRecipeById(int id, CancellationToken token)
        {
            string key = GetKeyString(id);
            GetRecipeByIdDTO? mappedRecipe = await _cache.Cache.GetOrCreateAsync(key, async (entry) =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromHours(3));
                entry.SetSlidingExpiration(TimeSpan.FromHours(1));
                entry.SetSize(1);
                Recipe requestedRecipe = await CheckAndReturnRecipe(id, token);
                GetRecipeByIdDTO recipeByIdDTO = _mapper.Map<GetRecipeByIdDTO>(requestedRecipe);
                return recipeByIdDTO!;
            });

            return mappedRecipe!;

        }

        public async Task<List<GetRecipeDTO>> GetRecipes(Filter filterOptions, CancellationToken token)
        {
            List<Recipe> requestedRecipies = await _recipeRepo.GetRecipes(filterOptions, token);

            List<GetRecipeDTO> mappedRecipies = _mapper.Map<List<GetRecipeDTO>>(requestedRecipies);

            return mappedRecipies;
        }

        async Task<Recipe> CheckAndReturnRecipe(int id, CancellationToken token)
        {
            Recipe? requestedRecipe = await _recipeRepo.GetRecipeByIdAsync(id, token);

            if (requestedRecipe is null)
            {
                throw new EntityNotFoundException($"Entity with provided id {id} wasn't found.");
            }

            return requestedRecipe;
        }

        private string GetKeyString(int id) => $"Recipe:{id}";
    }
}