using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookApp.Application.DTOs.RecipeDTOs;
using CookApp.Model.Entities;

namespace CookApp.Application.MapProfiles
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {

            CreateMap<Ingredient, string>().ConvertUsing(ingr => ingr.IngredientName);

            CreateMap<Recipe, GetRecipeDTO>()
            .ForMember(dest => dest.RecipeName, options => options.MapFrom(from => from.RecipeName))
            .ForMember(
            dest => dest.Ingredients,
            opt => opt.MapFrom(src => src.Ingredients.Select(i => i.IngredientName
            )));

            CreateMap<Recipe, GetRecipeByIdDTO>()
            .ForMember(dest=>dest.RecipeName, options=>options.MapFrom(from=>from.RecipeName))
            .ForMember(
            dest => dest.Ingredients,
            opt => opt.MapFrom(src => src.Ingredients.Select(ingr => ingr.IngredientName
            )));
        }
    }
}