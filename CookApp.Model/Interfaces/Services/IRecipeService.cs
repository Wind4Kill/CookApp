using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookApp.Model.DTOs.RecipeDTOs;

namespace CookApp.Model.Interfaces.Services
{
    public interface IRecipeService
    {
        public Task<List<GetRecipeDTO>> GetRecipes();
    }
}