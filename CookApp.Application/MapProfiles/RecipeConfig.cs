using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookApp.Application.DTOs.RecipeDTOs;
using CookApp.Model.Entities;

namespace CookApp.Application.MapProfiles
{
    public class RecipeConfig : Profile
    {
        public RecipeConfig()
        {
            CreateMap<string, Ingredient>().ConvertUsing(ingredients => new Ingredient() { IngredientName = ingredients });
            CreateMap<CreateRecipeDTO, Recipe>()
            .ForMember(dest=>dest.RecipeName, options=>options.MapFrom(from=>from.RecipeName))
            .ForMember(dest => dest.Ingredients, options => options.MapFrom(src => src.Ingredients));
        }
    }
}