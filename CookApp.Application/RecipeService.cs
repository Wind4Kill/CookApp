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

namespace CookApp.Application
{
    public class RecipeService : IRecipeService
    {
        readonly IRecipeRepository _recipeRepo;
        readonly IMapper _mapper;

        public RecipeService(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepo = recipeRepository;
            _mapper = mapper;
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
            Recipe? requestedRecipe = await _recipeRepo.GetRecipeByIdAsync(id);
            if (requestedRecipe is null)
            {
                throw new EntityNotFoundException("Recipe with specified ID couldn't be found.");
            }
           return await _recipeRepo.DeleteRecipe(requestedRecipe);
        }

        public async Task<GetRecipeByIdDTO> GetRecipeById(int id)
        {
            Recipe? requestedRecipe = await _recipeRepo.GetRecipeByIdAsync(id);

            if (requestedRecipe is null)
            {
                throw new EntityNotFoundException("Recipe with such ID couldn't be found.");
            }

            return _mapper.Map<GetRecipeByIdDTO>(requestedRecipe);


        }

        public async Task<List<GetRecipeDTO>> GetRecipes(Filter filterOptions)
        {
            IQueryable<Recipe> processedRecipies = _recipeRepo.GetRecipes().
            OrderRecipes(filterOptions.OrderType).
            FilterRecipes(filterOptions.FiltrationType, filterOptions.FiltrationData).
            Paginate(filterOptions.Page);
            

            return await _mapper.ProjectTo<GetRecipeDTO>(processedRecipies).ToListAsync();
        }
    }
}