using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookApp.Model.DTOs;
using CookApp.Model.DTOs.RecipeDTOs;
using CookApp.Model.FiltrationClasses;

namespace CookApp.Model.Interfaces.Services
{
    public interface IRecipeService
    {
        public Task<List<GetRecipeDTO>> GetRecipes(Filter filterOptions);

        public Task<GetRecipeByIdDTO> GetRecipeById(int id);

        public Task<Recipe> CreateRecipe(CreateRecipeDTO recipeDTO);

        public Task<int> DeleteRecipe(int id);
    }
}