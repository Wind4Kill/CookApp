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
        public Task<List<GetRecipeDTO>> GetRecipes(Filter filterOptions, CancellationToken token);

        public Task<GetRecipeByIdDTO> GetRecipeById(int id, CancellationToken token);

        public Task<GetRecipeByIdDTO> CreateRecipe(CreateRecipeDTO recipeDTO, CancellationToken token);

        public Task DeleteRecipe(int id, CancellationToken token);
    }
}