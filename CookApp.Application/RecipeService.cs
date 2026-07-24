using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookApp.Data.Repositories;
using CookApp.Model;
using CookApp.Model.DTOs.RecipeDTOs;
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
        public async Task<List<GetRecipeDTO>> GetRecipes()
        {
            var recipes = _recipeRepo.GetRecipes();
            return await _mapper.ProjectTo<GetRecipeDTO>(recipes).ToListAsync();
        }
    }
}