using AutoMapper;
using CookApp.Application.DTOs.RecipeDTOs;
using CookApp.Application.FiltrationClasses;
using CookApp.Application.Interfaces.Caching;
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

        readonly ICacheService<Recipe> _cache;

        public RecipeService(IRecipeRepository recipeRepository, IMapper mapper, ICacheService<Recipe> cache)
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
            await _recipeRepo.DeleteRecipe(requestedRecipe, token);
            await _cache.RemoveAsync(id, token);
        }

        public async Task<GetRecipeByIdDTO> GetRecipeById(int id, CancellationToken token)
        {
            Recipe? requestedRecipe = await _cache.GetValueAsync(id, token);
            if (requestedRecipe is null)
            {
                requestedRecipe = await CheckAndReturnRecipe(id, token);
                await _cache.AddValueAsync(requestedRecipe, requestedRecipe.RecipeId, token);
            }
            
            GetRecipeByIdDTO mappedRecipe = _mapper.Map<GetRecipeByIdDTO>(requestedRecipe);
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

    }
}