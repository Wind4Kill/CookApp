using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookApp.Model.Entities;

namespace CookApp.Model.DTOs.AutoMapperConfigs
{
    public class RecipeConfig:Profile
    {
        public RecipeConfig()
        {
            CreateMap<string, Ingredient>().ConvertUsing(ingredients => new Ingredient(){IngredientName=ingredients});
            CreateMap<CreateRecipeDTO, Recipe>().ForMember(dest => dest.Ingredients, options => options.MapFrom(src => src.Ingredients));
        }
    }
}