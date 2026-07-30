using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookApp.Data.Repositories;
using CookApp.Model;
using CookApp.Model.DTOs;
using CookApp.Model.DTOs.RecipeDTOs;
using CookApp.Model.Entities;
using CookApp.Model.Exceptions;
using CookApp.Model.FiltrationClasses;
using CookApp.Model.Interfaces;
using CookApp.Model.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Recipe> CreateRecipe(CreateRecipeDTO recipeDTO)
        {
            Ingredient[] recipeIngredients = recipeDTO.Ingredients.Select(ingrName => new Ingredient() { IngredientName = ingrName }).ToArray();
            Recipe createdRecipe = new Recipe() { RecipeName = recipeDTO.RecipeName, Ingredients = new() };
            createdRecipe.Ingredients.AddRange(recipeIngredients);

            return await _recipeRepo.CreateRecipeAsync(createdRecipe);
        }

        public async Task<int> DeleteRecipe(int id)
        {
            Recipe requestedRecipe = await CheckAndReturnRecipe(id);
            string key = $"Book:{id}";
            _cache.Cache.Remove(key);
            return await _recipeRepo.DeleteRecipe(requestedRecipe);
        }

        public async Task<GetRecipeByIdDTO> GetRecipeById(int id)
        {
            string key = $"Recipe:{id}";
            GetRecipeByIdDTO? mappedRecipe = await _cache.Cache.GetOrCreateAsync(key, async (entry) =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromHours(3));
                entry.SetSlidingExpiration(TimeSpan.FromHours(1));
                entry.SetSize(1);
                Recipe requestedRecipe = await CheckAndReturnRecipe(id);
                GetRecipeByIdDTO recipeByIdDTO = _mapper.Map<GetRecipeByIdDTO>(requestedRecipe);
                return recipeByIdDTO!;
            });

            return mappedRecipe!;

        }

        public async Task<List<GetRecipeDTO>> GetRecipes(Filter filterOptions)
        {
            IQueryable<Recipe> processedRecipies = _recipeRepo.GetRecipes().
            OrderRecipes(filterOptions.OrderType).
            FilterRecipes(filterOptions.FiltrationType, filterOptions.FiltrationData).
            Paginate(filterOptions.Page);


            return await _mapper.ProjectTo<GetRecipeDTO>(processedRecipies).ToListAsync();
        }

        public async Task<Recipe> CheckAndReturnRecipe(int id)
        {
            Recipe? requestedRecipe = await _recipeRepo.GetRecipeByIdAsync(id);

            if (requestedRecipe is null)
            {
                throw new EntityNotFoundException($"Entity with provided id {id} wasn't found.");
            }

            return requestedRecipe;
        }
    }
}